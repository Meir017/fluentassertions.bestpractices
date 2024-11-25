using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Analyzers.TestUtils;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace FluentAssertions.Analyzers.Tests;

public class CodeFixVerifierArguments : CsProjectArguments
{
    public List<string> FixedSources { get; } = new();

    public List<DiagnosticAnalyzer> DiagnosticAnalyzers { get; } = new();

    public List<CodeFixProvider> CodeFixProviders { get; } = new();

    public CodeFixVerifierArguments() { }

    public CodeFixVerifierArguments WithDiagnosticAnalyzer<TDiagnosticAnalyzer>() where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
    {
        DiagnosticAnalyzers.Add(new TDiagnosticAnalyzer());
        return this;
    }

    public CodeFixVerifierArguments WithCodeFixProvider<TCodeFixProvider>() where TCodeFixProvider : CodeFixProvider, new()
    {
        CodeFixProviders.Add(new TCodeFixProvider());
        return this;
    }

    public CodeFixVerifierArguments WithFixedSources(params string[] fixedSources)
    {
        FixedSources.AddRange(fixedSources);
        return this;
    }
}

public class CodeFixVerifierNewArguments<TCodeFix, TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFix : CodeFixProvider, new()
{
    public List<string> Sources { get; } = new();
    public List<string> FixedSources { get; } = new();
    public List<PackageIdentity> PackageReferences { get; } = new();
    public DiagnosticResult ExpectedDiagnostic { get; private set; }

    public CodeFixVerifierNewArguments(DiagnosticResult expectedDiagnostic) => ExpectedDiagnostic = expectedDiagnostic;

    public CodeFixVerifierNewArguments<TCodeFix, TAnalyzer> WithPackages(params PackageReference[] packages)
    {
        PackageReferences.AddRange(packages.Select(x => new PackageIdentity(x.Name, x.Version)));
        return this;
    }
}