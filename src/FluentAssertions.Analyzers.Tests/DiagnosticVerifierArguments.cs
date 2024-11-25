using System.Collections.Generic;
using FluentAssertions.Analyzers.TestUtils;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentAssertions.Analyzers.Tests;

public class DiagnosticVerifierArguments : CsProjectArguments
{
    public List<LegacyDiagnosticResult> ExpectedDiagnostics { get; } = new();

    public List<DiagnosticAnalyzer> DiagnosticAnalyzers { get; } = new();

    public DiagnosticVerifierArguments() { }

    public DiagnosticVerifierArguments WithDiagnosticAnalyzer<TDiagnosticAnalyzer>() where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
    {
        DiagnosticAnalyzers.Add(new TDiagnosticAnalyzer());
        return this;
    }

    public DiagnosticVerifierArguments WithAllAnalyzers()
    {
        DiagnosticAnalyzers.Clear();
        DiagnosticAnalyzers.AddRange(CodeAnalyzersUtils.GetAllAnalyzers());
        return this;
    }

    public DiagnosticVerifierArguments WithExpectedDiagnostics(params LegacyDiagnosticResult[] expectedDiagnostics)
    {
        ExpectedDiagnostics.AddRange(expectedDiagnostics);
        return this;
    }
}
