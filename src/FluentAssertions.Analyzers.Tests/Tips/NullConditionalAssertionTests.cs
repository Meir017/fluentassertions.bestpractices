using FluentAssertions.Analyzers.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace FluentAssertions.Analyzers.Tests.Tips
{
    [TestClass]
    public class NullConditionalAssertionTests
    {
        [DataTestMethod]
        [AssertionDiagnostic("actual?.Should().Be(expected{0});")]
        [AssertionDiagnostic("actual?.MyProperty.Should().Be(\"test\"{0});")]
        [AssertionDiagnostic("actual.MyProperty?.Should().Be(\"test\"{0});")]
        [AssertionDiagnostic("(actual.MyProperty)?.Should().Be(\"test\"{0});")]
        [AssertionDiagnostic("(actual?.MyProperty)?.Should().Be(\"test\"{0});")]
        [AssertionDiagnostic("actual?.MyProperty.Should().Be(actual?.MyProperty{0});")]
        [AssertionDiagnostic("actual.MyList?.Where(obj => obj?.ToString() == null).Count().Should().Be(0{0});")]
        [Implemented]
        public void NullConditionalMayNotExecuteTest(string assertion)
        {
            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithDiagnosticAnalyzer<FluentAssertionsAnalyzer>()
                .WithSources(Code(assertion))
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
                .WithExpectedDiagnostics(new LegacyDiagnosticResult
                {
                    Id = FluentAssertionsAnalyzer.DiagnosticId,
                    Message = DiagnosticMetadata.NullConditionalMayNotExecute.Message,
                    Severity = Microsoft.CodeAnalysis.DiagnosticSeverity.Info, // TODO: change to warning
                    VisitorName = nameof(DiagnosticMetadata.NullConditionalMayNotExecute),
                    Locations = new DiagnosticResultLocation[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 11, 13)
                    }
                })
            );
        }

        [DataTestMethod]
        [AssertionDiagnostic("(actual?.MyProperty).Should().Be(\"test\"{0});")]
        [AssertionDiagnostic("actual.MyProperty.Should().Be(actual?.MyProperty{0});")]
        [AssertionDiagnostic("actual.MyList.Where(obj => obj?.ToString() == null).Should().HaveCount(6{0});")]
        [Implemented]
        public void NullConditionalWillStillExecuteTest(string assertion)
        {
            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithDiagnosticAnalyzer<FluentAssertionsAnalyzer>()
                .WithSources(Code(assertion))
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
            );
        }

        private static string Code(string assertion) =>
            new StringBuilder()
                .AppendLine("using System;")
                .AppendLine("using System.Collections.Generic;")
                .AppendLine("using System.Linq;")
                .AppendLine("using FluentAssertions;using FluentAssertions.Extensions;")
                .AppendLine("namespace TestNamespace")
                .AppendLine("{")
                .AppendLine("    class TestClass")
                .AppendLine("    {")
                .AppendLine("        void TestMethod(MyClass actual, MyClass expected)")
                .AppendLine("        {")
                .AppendLine($"            {assertion}")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("    class MyClass")
                .AppendLine("    {")
                .AppendLine("        public string MyProperty { get; set; }")
                .AppendLine("        public List<object> MyList { get; set; }")
                .AppendLine("    }")
                .AppendLine("}")
                .ToString();
    }
}
