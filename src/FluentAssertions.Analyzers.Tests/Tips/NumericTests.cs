﻿using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests.Tips
{
    [TestClass]
    public class NumericTests
    {
        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Should().BeGreaterThan(0{0});")]
        [AssertionDiagnostic("actual.Should().BeGreaterThan(0{0}).ToString();")]
        [Implemented]
        public void NumericShouldBePositive_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<NumericShouldBePositiveAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().BeGreaterThan(0{0});",
            newAssertion: "actual.Should().BePositive({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().BeGreaterThan(0{0}).ToString();",
            newAssertion: "actual.Should().BePositive({0}).ToString();")]
        [Implemented]
        public void NumericShouldBePositive_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<NumericShouldBePositiveCodeFix, NumericShouldBePositiveAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Should().BeLessThan(0{0});")]
        [AssertionDiagnostic("actual.Should().BeLessThan(0{0}).ToString();")]
        [Implemented]
        public void NumericShouldBeNegative_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<NumericShouldBeNegativeAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().BeLessThan(0{0});",
            newAssertion: "actual.Should().BeNegative({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().BeLessThan(0{0}).ToString();",
            newAssertion: "actual.Should().BeNegative({0}).ToString();")]
        [Implemented]
        public void NumericShouldBeNegative_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<NumericShouldBeNegativeCodeFix, NumericShouldBeNegativeAnalyzer>(oldAssertion, newAssertion);

        private void VerifyCSharpDiagnostic<TDiagnosticAnalyzer>(string sourceAssertion) where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var source = GenerateCode.NumericAssertion(sourceAssertion);

            var type = typeof(TDiagnosticAnalyzer);
            var diagnosticId = (string)type.GetField("DiagnosticId").GetValue(null);
            var message = (string)type.GetField("Message").GetValue(null);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new DiagnosticResult
            {
                Id = diagnosticId,
                Message = message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 10, 13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(string oldSourceAssertion, string newSourceAssertion)
            where TCodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider, new()
            where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var oldSource = GenerateCode.NumericAssertion(oldSourceAssertion);
            var newSource = GenerateCode.NumericAssertion(newSourceAssertion);

            DiagnosticVerifier.VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(oldSource, newSource);
        }
    }
}
