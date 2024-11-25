using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentAssertions.Execution;
using FluentAssertions.Analyzers.TestUtils;
using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Threading;
using Microsoft.CodeAnalysis.Simplification;
using Microsoft.CodeAnalysis.Formatting;

namespace FluentAssertions.Analyzers.Tests
{
    /// <summary>
    /// Superclass of all Unit Tests for DiagnosticAnalyzers
    /// </summary>
    public static class DiagnosticVerifier
    {
        #region CodeFixVerifier

        public static void VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(string oldSource, string newSource)
            where TCodeFixProvider : CodeFixProvider, new()
            where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
        {
            VerifyFix(new CodeFixVerifierArguments()
                .WithDiagnosticAnalyzer<TDiagnosticAnalyzer>()
                .WithCodeFixProvider<TCodeFixProvider>()
                .WithSources(oldSource)
                .WithFixedSources(newSource)
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
            );
        }

        public static void VerifyFix(CodeFixVerifierArguments arguments)
            => VerifyFix(arguments, arguments.DiagnosticAnalyzers.Single(), arguments.CodeFixProviders.Single(), arguments.FixedSources.Single());

        public static void VerifyNoFix(CodeFixVerifierArguments arguments)
            => VerifyNoFix(arguments, arguments.DiagnosticAnalyzers.Single(), arguments.CodeFixProviders.Single());
        private static void VerifyNoFix(CsProjectArguments arguments, DiagnosticAnalyzer analyzer, CodeFixProvider codeFixProvider)
        {
            var document = CsProjectGenerator.CreateDocument(arguments);
            var analyzerDiagnostics = GetSortedDiagnosticsFromDocuments(new[] { analyzer }, new[] { document });
            var compilerDiagnostics = GetCompilerDiagnostics(document);
            var attempts = analyzerDiagnostics.Length;

            for (int i = 0; i < attempts; ++i)
            {
                var actions = new List<CodeAction>();
                var context = new CodeFixContext(document, analyzerDiagnostics[0], (a, d) => actions.Add(a), CancellationToken.None);
                codeFixProvider.RegisterCodeFixesAsync(context).Wait();

                actions.Should().BeEmpty("There should be no code fix actions available to suppress the diagnostic.");
            }
        }

        /// <summary>
        /// General verifier for codefixes.
        /// Creates a Document from the source string, then gets diagnostics on it and applies the relevant codefixes.
        /// Then gets the string after the codefix is applied and compares it with the expected result.
        /// Note: If any codefix causes new diagnostics to show up, the test fails unless allowNewCompilerDiagnostics is set to true.
        /// </summary>
        /// <param name="language">The language the source code is in</param>
        /// <param name="analyzer">The analyzer to be applied to the source code</param>
        /// <param name="codeFixProvider">The codefix to be applied to the code wherever the relevant Diagnostic is found</param>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it</param>
        /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied</param>
        private static void VerifyFix(CsProjectArguments arguments, DiagnosticAnalyzer analyzer, CodeFixProvider codeFixProvider, string newSource)
        {
            var document = CsProjectGenerator.CreateDocument(arguments);
            var analyzerDiagnostics = GetSortedDiagnosticsFromDocuments(new[] { analyzer }, new[] { document });
            var compilerDiagnostics = GetCompilerDiagnostics(document);
            var attempts = analyzerDiagnostics.Length;

            for (int i = 0; i < attempts; ++i)
            {
                var actions = new List<CodeAction>();
                var context = new CodeFixContext(document, analyzerDiagnostics[0], (a, d) => actions.Add(a), CancellationToken.None);
                codeFixProvider.RegisterCodeFixesAsync(context).Wait();

                if (actions.Count == 0)
                {
                    break;
                }

                document = ApplyFix(document, actions[0]);
                analyzerDiagnostics = GetSortedDiagnosticsFromDocuments(new[] { analyzer }, new[] { document });

                var newCompilerDiagnostics = GetNewDiagnostics(compilerDiagnostics, GetCompilerDiagnostics(document));

                //check if applying the code fix introduced any new compiler diagnostics
                if (newCompilerDiagnostics.Any())
                {
                    // Format and get the compiler diagnostics again so that the locations make sense in the output
                    document = document.WithSyntaxRoot(Formatter.Format(document.GetSyntaxRootAsync().Result, Formatter.Annotation, document.Project.Solution.Workspace));
                    newCompilerDiagnostics = GetNewDiagnostics(compilerDiagnostics, GetCompilerDiagnostics(document));

                    throw new AssertionFailedException(string.Format("Fix introduced new compiler diagnostics:\r\n{0}\r\n\r\nNew document:\r\n{1}\r\n",
                        string.Join("\r\n", newCompilerDiagnostics.Select(d => d.ToString())),
                        document.GetSyntaxRootAsync().Result.ToFullString()));
                }

                //check if there are analyzer diagnostics left after the code fix
                if (analyzerDiagnostics.Length > 0)
                {
                    break;
                }
            }

            codeFixProvider.FixableDiagnosticIds.Should().BeSubsetOf(analyzer.SupportedDiagnostics.Select(d => d.Id));

            //after applying all of the code fixes, compare the resulting string to the inputted one
            var actual = GetStringFromDocument(document);
            actual.Should().Be(newSource);
        }

        /// <summary>
        /// Apply the inputted CodeAction to the inputted document.
        /// Meant to be used to apply codefixes.
        /// </summary>
        /// <param name="document">The Document to apply the fix on</param>
        /// <param name="codeAction">A CodeAction that will be applied to the Document.</param>
        /// <returns>A Document with the changes from the CodeAction</returns>
        private static Document ApplyFix(Document document, CodeAction codeAction)
        {
            var operations = codeAction.GetOperationsAsync(CancellationToken.None).Result;
            var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
            return solution.GetDocument(document.Id);
        }

        /// <summary>
        /// Compare two collections of Diagnostics,and return a list of any new diagnostics that appear only in the second collection.
        /// Note: Considers Diagnostics to be the same if they have the same Ids.  In the case of multiple diagnostics with the same Id in a row,
        /// this method may not necessarily return the new one.
        /// </summary>
        /// <param name="diagnostics">The Diagnostics that existed in the code before the CodeFix was applied</param>
        /// <param name="newDiagnostics">The Diagnostics that exist in the code after the CodeFix was applied</param>
        /// <returns>A list of Diagnostics that only surfaced in the code after the CodeFix was applied</returns>
        private static IEnumerable<Diagnostic> GetNewDiagnostics(IEnumerable<Diagnostic> diagnostics, IEnumerable<Diagnostic> newDiagnostics)
        {
            var oldArray = diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
            var newArray = newDiagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();

            int oldIndex = 0;
            int newIndex = 0;

            while (newIndex < newArray.Length)
            {
                if (oldIndex < oldArray.Length && oldArray[oldIndex].Id == newArray[newIndex].Id)
                {
                    ++oldIndex;
                    ++newIndex;
                }
                else
                {
                    yield return newArray[newIndex++];
                }
            }
        }

        /// <summary>
        /// Get the existing compiler diagnostics on the inputted document.
        /// </summary>
        /// <param name="document">The Document to run the compiler diagnostic analyzers on</param>
        /// <returns>The compiler diagnostics that were found in the code</returns>
        private static IEnumerable<Diagnostic> GetCompilerDiagnostics(Document document)
        {
            var compilation = document.GetSemanticModelAsync().Result.Compilation;
            return compilation.WithOptions(compilation.Options
                .WithSpecificDiagnosticOptions(new Dictionary<string, ReportDiagnostic>
                {
                    ["CS1701"] = ReportDiagnostic.Suppress, // Binding redirects
                    ["CS1702"] = ReportDiagnostic.Suppress,
                    ["CS1705"] = ReportDiagnostic.Suppress,
                    ["CS8019"] = ReportDiagnostic.Suppress // TODO: Unnecessary using directive
                })
            ).GetDiagnostics();
        }

        /// <summary>
        /// Given a document, turn it into a string based on the syntax root
        /// </summary>
        /// <param name="document">The Document to be converted to a string</param>
        /// <returns>A string containing the syntax of the Document after formatting</returns>
        private static string GetStringFromDocument(Document document)
        {
            var simplifiedDoc = Simplifier.ReduceAsync(document, Simplifier.Annotation).Result;
            var root = simplifiedDoc.GetSyntaxRootAsync().Result;
            root = Formatter.Format(root, Formatter.Annotation, simplifiedDoc.Project.Solution.Workspace);
            return root.GetText().ToString();
        }

        #endregion        

        #region  Get Diagnostics

        /// <summary>
        /// Given an analyzer and a document to apply it to, run the analyzer and gather an array of diagnostics found in it.
        /// The returned diagnostics are then ordered by location in the source document.
        /// </summary>
        /// <param name="analyzers">The analyzer to run on the documents</param>
        /// <param name="documents">The Documents that the analyzer will be run on</param>
        /// <returns>An IEnumerable of Diagnostics that surfaced in the source code, sorted by Location</returns>
        private static Diagnostic[] GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer[] analyzers, Document[] documents) => GetSortedDiagnosticsFromDocuments(analyzers, TestAnalyzerConfigOptionsProvider.Empty, documents);

        private static Diagnostic[] GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer[] analyzers, AnalyzerConfigOptionsProvider analyzerConfigOptions, Document[] documents)
        {
            var projects = new HashSet<Project>();
            foreach (var document in documents)
            {
                projects.Add(document.Project);
            }

            var diagnostics = new List<Diagnostic>();
            foreach (var project in projects)
            {
                var compilation = project.GetCompilationAsync().Result;
                var compilationWithAnalyzers = compilation
                    .WithOptions(compilation.Options
                        .WithOutputKind(OutputKind.DynamicallyLinkedLibrary)
                        .WithSpecificDiagnosticOptions(new Dictionary<string, ReportDiagnostic>
                        {
                            ["CS1701"] = ReportDiagnostic.Suppress, // Binding redirects
                            ["CS1702"] = ReportDiagnostic.Suppress,
                            ["CS1705"] = ReportDiagnostic.Suppress,
                            ["CS8019"] = ReportDiagnostic.Suppress // TODO: Unnecessary using directive
                        }))
                    .WithAnalyzers(ImmutableArray.Create(analyzers), new AnalyzerOptions(ImmutableArray<AdditionalText>.Empty, analyzerConfigOptions));
                var relevantDiagnostics = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;

                var allDiagnostics = compilationWithAnalyzers.GetAllDiagnosticsAsync().Result;
                var other = allDiagnostics.Except(relevantDiagnostics).ToArray();

                var code = documents[0].GetSyntaxRootAsync().Result.ToFullString();

                other.Should().BeEmpty("there should be no error diagnostics that are not related to the test.{0}code: {1}", Environment.NewLine, code);

                foreach (var diag in relevantDiagnostics)
                {
                    if (diag.Location == Location.None || diag.Location.IsInMetadata)
                    {
                        diagnostics.Add(diag);
                    }
                    else
                    {
                        for (int i = 0; i < documents.Length; i++)
                        {
                            var document = documents[i];
                            var tree = document.GetSyntaxTreeAsync().Result;
                            if (tree == diag.Location.SourceTree)
                            {
                                diagnostics.Add(diag);
                            }
                        }
                    }
                }
            }

            var results = diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
            diagnostics.Clear();
            return results;
        }

        #endregion

        #region Verifier wrappers

        /// <summary>
        /// Called to test a C# DiagnosticAnalyzer when applied on the single inputted string as a source
        /// Note: input a DiagnosticResult for each Diagnostic expected
        /// </summary>
        /// <param name="source">A class in the form of a string to run the analyzer on</param>
        /// <param name="expected"> DiagnosticResults that should appear after the analyzer is run on the source</param>
        public static void VerifyCSharpDiagnostic<TDiagnosticAnalyzer>(string source, params DiagnosticResult[] expected) where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
        {
            VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithDiagnosticAnalyzer<TDiagnosticAnalyzer>()
                .WithSources(source)
                .WithExpectedDiagnostics(expected));
        }

        public static void VerifyDiagnostic(DiagnosticVerifierArguments arguments)
        {
            var project = CsProjectGenerator.CreateProject(arguments);
            var documents = project.Documents.ToArray();

            var diagnostics = GetSortedDiagnosticsFromDocuments(arguments.DiagnosticAnalyzers.ToArray(), arguments.CreateAnalyzerConfigOptionsProvider(), documents);
            VerifyDiagnosticResults(diagnostics, arguments.DiagnosticAnalyzers.ToArray(), arguments.ExpectedDiagnostics.ToArray());
        }

        public static void VerifyCSharpDiagnosticUsingAllAnalyzers(string source, params DiagnosticResult[] expected)
        {
            VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithAllAnalyzers()
                .WithSources(source)
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
                .WithExpectedDiagnostics(expected));
        }

        #endregion

        #region Actual comparisons and verifications

        private static void VerifyDiagnosticResults(IEnumerable<Diagnostic> actualResults, DiagnosticAnalyzer[] analyzers, params DiagnosticResult[] expectedResults)
        {
            int expectedCount = expectedResults.Length;
            int actualCount = actualResults.Count();

            if (expectedCount != actualCount)
            {
                string diagnosticsOutput = actualResults.Any() ? FormatDiagnostics(analyzers, actualResults.ToArray()) : "    NONE.";

                throw new AssertionFailedException(
                    string.Format("Mismatch between number of diagnostics returned, expected \"{0}\" actual \"{1}\"\r\n\r\nDiagnostics:\r\n{2}\r\n", expectedCount, actualCount, diagnosticsOutput));
            }

            for (int i = 0; i < expectedResults.Length; i++)
            {
                var actual = actualResults.ElementAt(i);
                var expected = expectedResults[i];

                if (expected.Line == -1 && expected.Column == -1)
                {
                    if (actual.Location != Location.None)
                    {
                        throw new AssertionFailedException(
                            string.Format("Expected:\nA project diagnostic with No location\nActual:\n{0}",
                            FormatDiagnostics(analyzers, actual)));
                    }
                }
                else
                {
                    VerifyDiagnosticLocation(analyzers, actual, actual.Location, expected.Locations.First());
                    var additionalLocations = actual.AdditionalLocations.ToArray();

                    if (additionalLocations.Length != expected.Locations.Length - 1)
                    {
                        throw new AssertionFailedException(
                            string.Format("Expected {0} additional locations but got {1} for Diagnostic:\r\n    {2}\r\n",
                                expected.Locations.Length - 1, additionalLocations.Length,
                                FormatDiagnostics(analyzers, actual)));
                    }

                    for (int j = 0; j < additionalLocations.Length; ++j)
                    {
                        VerifyDiagnosticLocation(analyzers, actual, additionalLocations[j], expected.Locations[j + 1]);
                    }
                }

                if (actual.Id != expected.Id)
                {
                    throw new AssertionFailedException(
                        string.Format("Expected diagnostic id to be \"{0}\" was \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n",
                            expected.Id, actual.Id, FormatDiagnostics(analyzers, actual)));
                }

                if (actual.Severity != expected.Severity)
                {
                    throw new AssertionFailedException(
                        string.Format("Expected diagnostic severity to be \"{0}\" was \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n",
                            expected.Severity, actual.Severity, FormatDiagnostics(analyzers, actual)));
                }

                if (actual.GetMessage() != expected.Message)
                {
                    throw new AssertionFailedException(
                        string.Format("Expected diagnostic message to be \"{0}\" was \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n",
                            expected.Message, actual.GetMessage(), FormatDiagnostics(analyzers, actual)));
                }

                if (expected.VisitorName != null && actual.Properties[Constants.DiagnosticProperties.VisitorName] != expected.VisitorName)
                {
                    throw new AssertionFailedException(
                        string.Format("Expected diagnostic visitorName property to be \"{0}\" was \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n",
                            expected.VisitorName, actual.Properties[Constants.DiagnosticProperties.VisitorName], FormatDiagnostics(analyzers, actual)));
                }
            }
        }

        /// <summary>
        /// Helper method to VerifyDiagnosticResult that checks the location of a diagnostic and compares it with the location in the expected DiagnosticResult.
        /// </summary>
        /// <param name="analyzers">The analyzer that was being run on the sources</param>
        /// <param name="diagnostic">The diagnostic that was found in the code</param>
        /// <param name="actual">The Location of the Diagnostic found in the code</param>
        /// <param name="expected">The DiagnosticResultLocation that should have been found</param>
        private static void VerifyDiagnosticLocation(DiagnosticAnalyzer[] analyzers, Diagnostic diagnostic, Location actual, DiagnosticResultLocation expected)
        {
            var actualSpan = actual.GetLineSpan();

            (actualSpan.Path == expected.Path || (actualSpan.Path != null && actualSpan.Path.Contains("Test0.") && expected.Path.Contains("Test.")))
                .Should().BeTrue(string.Format("Expected diagnostic to be in file \"{0}\" was actually in file \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n",
                    expected.Path, actualSpan.Path, FormatDiagnostics(analyzers, diagnostic)));

            var actualLinePosition = actualSpan.StartLinePosition;

            // Only check line position if there is an actual line in the real diagnostic
            if (actualLinePosition.Line > 0)
            {
                if (actualLinePosition.Line + 1 != expected.Line)
                {
                    throw new AssertionFailedException(string.Format("Expected diagnostic to be on line \"{0}\" was actually on line \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n",
                        expected.Line, actualLinePosition.Line + 1, FormatDiagnostics(analyzers, diagnostic)));
                }
            }

            // Only check column position if there is an actual column position in the real diagnostic
            if (actualLinePosition.Character > 0)
            {
                if (actualLinePosition.Character + 1 != expected.Column)
                {
                    throw new AssertionFailedException(
                        string.Format("Expected diagnostic to start at column \"{0}\" was actually at column \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n",
                            expected.Column, actualLinePosition.Character + 1, FormatDiagnostics(analyzers, diagnostic)));
                }
            }
        }
        #endregion

        #region Formatting Diagnostics
        /// <summary>
        /// Helper method to format a Diagnostic into an easily readable string
        /// </summary>
        /// <param name="analyzers">The analyzer that this verifier tests</param>
        /// <param name="diagnostics">The Diagnostics to be formatted</param>
        /// <returns>The Diagnostics formatted as a string</returns>
        private static string FormatDiagnostics(DiagnosticAnalyzer[] analyzers, params Diagnostic[] diagnostics)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < diagnostics.Length; ++i)
            {
                builder.AppendLine("// " + diagnostics[i]);

                foreach (var analyzer in analyzers)
                {
                    var analyzerType = analyzer.GetType();
                    foreach (var rule in analyzer.SupportedDiagnostics)
                    {
                        if (rule != null && rule.Id == diagnostics[i].Id)
                        {
                            var location = diagnostics[i].Location;
                            if (location == Location.None)
                            {
                                builder.AppendFormat("GetGlobalResult({0}.{1})", analyzerType.Name, rule.Id);
                            }
                            else
                            {
                                location.IsInSource.Should().BeTrue($"Test base does not currently handle diagnostics in metadata locations. Diagnostic in metadata: {diagnostics[i]}\r\n");

                                string resultMethodName = diagnostics[i].Location.SourceTree.FilePath.EndsWith(".cs") ? "GetCSharpResultAt" : "GetBasicResultAt";
                                var linePosition = diagnostics[i].Location.GetLineSpan().StartLinePosition;

                                builder.AppendFormat("{0}({1}, {2}, {3}.{4})",
                                    resultMethodName,
                                    linePosition.Line + 1,
                                    linePosition.Character + 1,
                                    analyzerType.Name,
                                    rule.Id);
                            }

                            if (i != diagnostics.Length - 1)
                            {
                                builder.Append(',');
                            }

                            builder.AppendLine();
                            break;
                        }
                    }
                }

            }
            return builder.ToString();
        }
        #endregion
    }
}
