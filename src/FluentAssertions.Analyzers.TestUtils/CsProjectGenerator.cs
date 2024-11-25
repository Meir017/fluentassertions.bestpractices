using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

using System.Net.Http;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace FluentAssertions.Analyzers.TestUtils
{
    public class CsProjectGenerator
    {
        private static readonly string DefaultFilePathPrefix = "Test";
        private static readonly string CSharpDefaultFileExt = "cs";
        private static readonly string VisualBasicDefaultExt = "vb";
        private static readonly string TestProjectName = "TestProject";

        public static Document CreateDocument(CsProjectArguments arguments) => CreateProject(arguments).Documents.First();

        public static Project CreateProject(CsProjectArguments arguments)
        {
            string fileNamePrefix = DefaultFilePathPrefix;
            string fileExt = arguments.Language is LanguageNames.CSharp ? CSharpDefaultFileExt : VisualBasicDefaultExt;

            var projectId = ProjectId.CreateNewId(debugName: TestProjectName);

            var solution = new AdhocWorkspace()
                .CurrentSolution
                .AddProject(projectId, TestProjectName, TestProjectName, arguments.Language);
            foreach (var package in arguments.PackageReferences)
            {
                solution = solution.AddPackageReference(projectId, package);
            }

            solution = solution.AddTargetFrameworkReference(projectId, arguments.TargetFramework);

            for (int i = 0; i < arguments.Sources.Length; i++)
            {
                var newFileName = fileNamePrefix + i + "." + fileExt;
                var documentId = DocumentId.CreateNewId(projectId, debugName: newFileName);
                solution = solution.AddDocument(documentId, newFileName, SourceText.From(arguments.Sources[i]));
            }
            return solution.GetProject(projectId);
        }
    }
}
