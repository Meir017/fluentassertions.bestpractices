using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;

namespace FluentAssertions.Analyzers.FluentAssertionAnalyzerDocsGenerator;

public class DocsGenerator
{
    public async Task Execute()
    {
        MSBuildLocator.RegisterDefaults();

        using var workspace = MSBuildWorkspace.Create();

        var project = await workspace.OpenProjectAsync(@"..\FluentAssertions.Analyzers.FluentAssertionAnalyzerDocs\FluentAssertions.Analyzers.FluentAssertionAnalyzerDocs.csproj");

        DiagnosticAnalyzer analyzer = new FluentAssertionsAnalyzer();
        var codeFixer = new FluentAssertionsCodeFixProvider();

        var compilation = await project.GetCompilationAsync();
        var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzer));

        var docs = new StringBuilder();

        docs.AppendLine("# FluentAssertions Analyzer Docs");

        foreach (var tree in compilationWithAnalyzers.Compilation.SyntaxTrees.Where(t => t.FilePath.EndsWith("Tests.cs")))
        {
            Console.WriteLine($"File: {Path.GetFileName(tree.FilePath)}");

            var root = await tree.GetRootAsync();
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methods)
            {
                docs.AppendLine($"## method: {method.Identifier}");
                docs.AppendLine();
                var bodyLines = method.Body.ToFullString().Split(Environment.NewLine)[1..^2];
                var paddingToRemove = bodyLines[0].IndexOf(bodyLines[0].TrimStart());
                var normalizedBody = bodyLines.Select(l => l.Length > paddingToRemove ? l.Substring(paddingToRemove) : l).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
                var methodBody = $"```cs{Environment.NewLine}{normalizedBody}{Environment.NewLine}```";
                docs.AppendLine(methodBody);
                docs.AppendLine();
            }

            var diagnostics = await compilationWithAnalyzers.GetAllDiagnosticsAsync();
            foreach (var diagnostic in diagnostics.Where(diagnostic => analyzer.SupportedDiagnostics.Any(d => d.Id == diagnostic.Id)))
            {
                Console.WriteLine($"source: {root.FindNode(diagnostic.Location.SourceSpan)}");
                Console.WriteLine($"  diagnostic: {diagnostic}");
            }
        }

        var docsPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "docs", "FluentAssertionsAnalyzer.md");
        Directory.CreateDirectory(Path.GetDirectoryName(docsPath));
        await File.WriteAllTextAsync(docsPath, docs.ToString());
    }
}
