using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Analyzers.TestUtils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class CollectionTests
    {
        private static CodeFixVerifierNewArguments<FluentAssertionsCodeFixProvider, FluentAssertionsAnalyzer> CreateCodeFixArguments(string source, string expected, DiagnosticMetadata metadata, LinePosition location)
            => new CodeFixVerifierNewArguments<FluentAssertionsCodeFixProvider, FluentAssertionsAnalyzer>(new DiagnosticResult(FluentAssertionsAnalyzer.DiagnosticId, DiagnosticSeverity.Info)
                .WithMessage(metadata.Message)
                .WithLocation(location)
                )
            {
                Sources = { source },
                FixedSources = { expected },
                
            }.WithPackages(PackageReference.FluentAssertions_6_12_0);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any().Should().BeTrue({0});",
            newAssertion: "actual.Should().NotBeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public async Task ExpressionBodyAssertion_TestCodeFix(string oldAssertion, string newAssertion)
        {
            var oldSource = GenerateCode.GenericIListExpressionBodyAssertion(oldAssertion);
            var newSource = GenerateCode.GenericIListExpressionBodyAssertion(newAssertion);

            var arguments = CreateCodeFixArguments(oldSource, newSource, DiagnosticMetadata.CollectionShouldNotBeEmpty_AnyShouldBeTrue, new LinePosition(9, 15));

            await DiagnosticVerifier.VerifyFixAsync(arguments);
        }

        [DataTestMethod]
        [AssertionDiagnostic("actual.Any().Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Any().Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Any().Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void CollectionsShouldNotBeEmpty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotBeEmpty_AnyShouldBeTrue);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any().Should().BeTrue({0});",
            newAssertion: "actual.Should().NotBeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionsShouldNotBeEmpty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Any().Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Any().Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Any().Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void CollectionsShouldNotBeEmpty_Array_TestAnalyzer(string assertion) => VerifyArrayCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotBeEmpty_AnyShouldBeTrue);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any().Should().BeTrue({0});",
            newAssertion: "actual.Should().NotBeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionsShouldNotBeEmpty_Array_TestCodeFix(string oldAssertion, string newAssertion) => VerifyArrayCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Any().Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any().Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Any().Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Any().Should().BeFalse({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeEmpty_AnyShouldBeFalse_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldBeEmpty_AnyShouldBeFalse);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveCount(0{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().HaveCount(0{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Should().HaveCount(0{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Should().HaveCount(0{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeEmpty_ShouldHaveCount0_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldBeEmpty_ShouldHaveCount0);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any().Should().BeFalse({0});",
            newAssertion: "actual.Should().BeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveCount(0{0});",
            newAssertion: "actual.Should().BeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any().Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveCount(0{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Any().Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().BeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Should().HaveCount(0{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().BeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Any().Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().BeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Should().HaveCount(0{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().BeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeEmpty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Any(x => x.BooleanProperty).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Any(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Any(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainProperty_AnyWithLambdaShouldBeTrue_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldContainProperty_AnyWithLambdaShouldBeTrue);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Where(x => x.BooleanProperty).Should().NotBeEmpty({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Where(x => x.BooleanProperty).Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Where(x => x.BooleanProperty).Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Where(x => x.BooleanProperty).Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainProperty_WhereShouldNotBeEmptyTestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldContainProperty_WhereShouldNotBeEmpty);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any(x => x.BooleanProperty).Should().BeTrue({0});",
            newAssertion: "actual.Should().Contain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Where(x => x.BooleanProperty).Should().NotBeEmpty({0});",
            newAssertion: "actual.Should().Contain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().Contain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Where(x => x.BooleanProperty).Should().NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().Contain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Any(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().Contain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Where(x => x.BooleanProperty).Should().NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().Contain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Any(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().Contain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Where(x => x.BooleanProperty).Should().NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().Contain(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainProperty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Any(x => x.BooleanProperty).Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any(x => x.BooleanProperty).Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Any(x => x.BooleanProperty).Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Any(x => x.BooleanProperty).Should().BeFalse({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainProperty_AnyLambdaShouldBeFalse_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotContainProperty_AnyLambdaShouldBeFalse);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Where(x => x.BooleanProperty).Should().BeEmpty({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Where(x => x.BooleanProperty).Should().BeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Where(x => x.BooleanProperty).Should().BeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Where(x => x.BooleanProperty).Should().BeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainProperty_WhereShouldBeEmpty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotContainProperty_WhereShouldBeEmpty);

        // TODO: all test cases are ignored
        // [DataTestMethod]
        [IgnoreAssertionDiagnostic("actual.Should().OnlyContain(x => !x.BooleanProperty{0});")]
        [IgnoreAssertionDiagnostic("actual.AsEnumerable().Should().OnlyContain(x => !x.BooleanProperty{0}).And.ToString();")]
        [IgnoreAssertionDiagnostic("actual.ToArray().Should().OnlyContain(x => !x.BooleanProperty{0}).And.ToString();")]
        [IgnoreAssertionDiagnostic("actual.ToList().Should().OnlyContain(x => !x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainProperty_ShouldOnlyContainNot_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotContainProperty_ShouldOnlyContainNot);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any(x => x.BooleanProperty).Should().BeFalse({0});",
            newAssertion: "actual.Should().NotContain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Where(x => x.BooleanProperty).Should().BeEmpty({0});",
            newAssertion: "actual.Should().NotContain(x => x.BooleanProperty{0});")]
        [IgnoreAssertionCodeFix(
            oldAssertion: "actual.Should().OnlyContain(x => !x.BooleanProperty{0});",
            newAssertion: "actual.Should().NotContain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any(x => x.BooleanProperty).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Where(x => x.BooleanProperty).Should().BeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContain(x => x.BooleanProperty{0}).And.ToString();")]
        [IgnoreAssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().OnlyContain(x => !x.BooleanProperty{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContain(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainProperty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.All(x => x.BooleanProperty).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().All(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().All(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().All(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyContainProperty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldOnlyContainProperty_AllShouldBeTrue);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.All(x => x.BooleanProperty).Should().BeTrue({0});",
            newAssertion: "actual.Should().OnlyContain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().All(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().OnlyContain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().All(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().OnlyContain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().All(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().OnlyContain(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyContainProperty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Contains(expectedItem).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Contains(expectedItem).Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Contains(expectedItem).Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Contains(expectedItem).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainItem_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldContainItem_ContainsShouldBeTrue);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Contains(expectedItem).Should().BeTrue({0});",
            newAssertion: "actual.Should().Contain(expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Contains(expectedItem).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().Contain(expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Contains(expectedItem).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().Contain(expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Contains(expectedItem).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().Contain(expectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainItem_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Contains(unexpectedItem).Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Contains(unexpectedItem).Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Contains(unexpectedItem).Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Contains(unexpectedItem).Should().BeFalse({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainItem_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotContainItem_ContainsShouldBeFalse);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Contains(unexpectedItem).Should().BeFalse({0});",
            newAssertion: "actual.Should().NotContain(unexpectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Contains(unexpectedItem).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContain(unexpectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Contains(unexpectedItem).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotContain(unexpectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Contains(unexpectedItem).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().NotContain(unexpectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainItem_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().Be(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().Be(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().Be(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().Be(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().Be(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().Be(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count.Should().Be(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count.Should().Be(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().Be(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().Be(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCount_CountShouldBe_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveCount_CountShouldBe);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().Be(0{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().Be(0{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().Be(0{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().Be(0{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCount_CountShouldBe0_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldBeEmpty_CountShouldBe0);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().Be(1{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().Be(1{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().Be(1{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().Be(1{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCount_CountShouldBe1_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldContainSingle_CountShouldBe1);

        [DataTestMethod]
        [AssertionDiagnostic("(array.Count() + 1).Should().Be(0{0}).And.ToString();")]
        [AssertionDiagnostic("(array.Count() + 1).Should().Be(1{0}).And.ToString();")]
        [AssertionDiagnostic("(array.Count() + 1).Should().Be(expectedSize{0}).And.ToString();")]
        [AssertionDiagnostic("(list.Count + 1).Should().Be(0{0}).And.ToString();")]
        [AssertionDiagnostic("(list.Count + 1).Should().Be(1{0}).And.ToString();")]
        [AssertionDiagnostic("(list.Count + 1).Should().Be(expectedSize{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCount_CountShouldBe_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(new StringBuilder()
            .AppendLine("using System;")
            .AppendLine("using System.Collections.Generic;")
            .AppendLine("using System.Linq;")
            .AppendLine("using FluentAssertions;")
            .AppendLine("using FluentAssertions.Extensions;")
            .AppendLine("namespace TestNamespace")
            .AppendLine("{")
            .AppendLine("    public class TestClass")
            .AppendLine("    {")
            .AppendLine("        public void TestMethod(string[] array, List<string> list, int expectedSize)")
            .AppendLine("       {")
            .AppendLine(assertion)
            .AppendLine("       }")
            .AppendLine("    }")
            .AppendLine("}")
            .ToString());

        [DataTestMethod]
        [AssertionDiagnostic(@"var array = new string[0, 0]; array.Length.Should().Be(0{0});")]
        [AssertionDiagnostic(@"var array = new string[0, 0, 0]; array.Length.Should().Be(0{0});")]
        [AssertionDiagnostic(@"var array = new string[0, 0, 0, 0]; array.Length.Should().Be(0{0});")]
        [AssertionDiagnostic(@"var array = new string[1, 1]; array.Length.Should().Be(0{0});")]
        [AssertionDiagnostic(@"var array = new string[2, 2]; array.Length.Should().Be(0{0});")]
        [AssertionDiagnostic(@"var array = new string[3, 3, 3]; array.Length.Should().Be(0{0});")]
        [AssertionDiagnostic(@"int[] array1 = [1, 2, 3]; int[] array2 = [4, 5, 6]; int[] both = [..array1, ..array2]; (array1.Length + array2.Length).Should().Be(both.Length{0});")]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/309")]
        public void CollectionShouldHaveCount_LengthShouldBe_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(new StringBuilder()
            .AppendLine("using System;")
            .AppendLine("using FluentAssertions;")
            .AppendLine("using FluentAssertions.Extensions;")
            .AppendLine("namespace TestNamespace")
            .AppendLine("{")
            .AppendLine("    public class TestClass")
            .AppendLine("    {")
            .AppendLine("        public void TestMethod()")
            .AppendLine("       {")
            .AppendLine(assertion)
            .AppendLine("       }")
            .AppendLine("    }")
            .AppendLine("}")
            .ToString());

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveCount(expected.Count() + 1{0});")]
        [AssertionDiagnostic("actual.Should().HaveCount(expected.Count() + unexpected.Count(){0});")]
        [AssertionDiagnostic("actual.Should().HaveCount(expected.Count + unexpected.Count{0});")]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/300")]
        public void CollectionShouldHaveCount_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().Be(k{0});",
            newAssertion: "actual.Should().HaveCount(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().Be(0{0});",
            newAssertion: "actual.Should().BeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().Be(1{0});",
            newAssertion: "actual.Should().ContainSingle({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().Be(6{0});",
            newAssertion: "actual.Should().HaveCount(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count.Should().Be(k{0});",
            newAssertion: "actual.ToList().Should().HaveCount(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count.Should().Be(0{0});",
            newAssertion: "actual.ToList().Should().BeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count.Should().Be(1{0});",
            newAssertion: "actual.ToList().Should().ContainSingle({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count.Should().Be(6{0});",
            newAssertion: "actual.ToList().Should().HaveCount(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Length.Should().Be(6{0});",
            newAssertion: "actual.ToArray().Should().HaveCount(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Length.Should().Be(k{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveCount(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().Be(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCount(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().Be(0{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().Be(1{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().ContainSingle({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().Be(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCount(6{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Length.Should().Be(6{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveCount(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCount_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().BeGreaterThan(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().BeGreaterThan(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeGreaterThan(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeGreaterThan(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterThan_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveCountGreaterThan_CountShouldBeGreaterThan);

        [DataTestMethod]
        [AssertionDiagnostic("(actual.Count() + 1).Should().BeGreaterThan(k{0});")]
        [AssertionDiagnostic("(actual.Count() + 1).Should().BeGreaterThan(6{0});")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterThan_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeGreaterThan(k{0});",
            newAssertion: "actual.Should().HaveCountGreaterThan(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeGreaterThan(6{0});",
            newAssertion: "actual.Should().HaveCountGreaterThan(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeGreaterThan(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountGreaterThan(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeGreaterThan(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountGreaterThan(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterThan_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().BeGreaterOrEqualTo(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().BeGreaterOrEqualTo(6{0});")]
        [AssertionDiagnostic("actual.Count.Should().BeGreaterOrEqualTo(k{0});")]
        [AssertionDiagnostic("actual.Count.Should().BeGreaterOrEqualTo(6{0});")]
        [AssertionDiagnostic("actual.ToArray().Length.Should().BeGreaterOrEqualTo(k{0});")]
        [AssertionDiagnostic("actual.ToArray().Length.Should().BeGreaterOrEqualTo(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeGreaterOrEqualTo(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeGreaterOrEqualTo(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterOrEqualTo_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveCountGreaterOrEqualTo_CountShouldBeGreaterOrEqualTo);

        [DataTestMethod]
        [AssertionDiagnostic("(actual.Count() + 1).Should().BeGreaterOrEqualTo(k{0});")]
        [AssertionDiagnostic("(actual.Count() + 1).Should().BeGreaterOrEqualTo(6{0});")]
        [AssertionDiagnostic("(actual.Count + 1).Should().BeGreaterOrEqualTo(k{0});")]
        [AssertionDiagnostic("(actual.Count + 1).Should().BeGreaterOrEqualTo(6{0});")]
        [AssertionDiagnostic("(actual.ToArray().Length + 1).Should().BeGreaterOrEqualTo(k{0});")]
        [AssertionDiagnostic("(actual.ToArray().Length + 1).Should().BeGreaterOrEqualTo(6{0});")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterOrEqualTo_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeGreaterOrEqualTo(k{0});",
            newAssertion: "actual.Should().HaveCountGreaterOrEqualTo(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeGreaterOrEqualTo(6{0});",
            newAssertion: "actual.Should().HaveCountGreaterOrEqualTo(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count.Should().BeGreaterOrEqualTo(k{0});",
            newAssertion: "actual.Should().HaveCountGreaterOrEqualTo(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count.Should().BeGreaterOrEqualTo(6{0});",
            newAssertion: "actual.Should().HaveCountGreaterOrEqualTo(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Length.Should().BeGreaterOrEqualTo(k{0});",
            newAssertion: "actual.ToArray().Should().HaveCountGreaterOrEqualTo(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Length.Should().BeGreaterOrEqualTo(6{0});",
            newAssertion: "actual.ToArray().Should().HaveCountGreaterOrEqualTo(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeGreaterOrEqualTo(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountGreaterOrEqualTo(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeGreaterOrEqualTo(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountGreaterOrEqualTo(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterOrEqualTo_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().BeLessThan(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().BeLessThan(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeLessThan(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeLessThan(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().BeLessThan(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().BeLessThan(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().BeLessThan(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().BeLessThan(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountLessThan_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveCountLessThan_CountShouldBeLessThan);

        [DataTestMethod]
        [AssertionDiagnostic("(actual.Count() + 1).Should().BeLessThan(k{0});")]
        [AssertionDiagnostic("(actual.Count() + 1).Should().BeLessThan(6{0});")]
        [Implemented]
        public void CollectionShouldHaveCountLessThan_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeLessThan(k{0});",
            newAssertion: "actual.Should().HaveCountLessThan(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeLessThan(6{0});",
            newAssertion: "actual.Should().HaveCountLessThan(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeLessThan(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountLessThan(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeLessThan(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountLessThan(6{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count().Should().BeLessThan(k{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().HaveCountLessThan(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count().Should().BeLessThan(6{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().HaveCountLessThan(6{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Count().Should().BeLessThan(k{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveCountLessThan(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Count().Should().BeLessThan(6{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveCountLessThan(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountLessThan_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().BeLessOrEqualTo(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().BeLessOrEqualTo(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeLessOrEqualTo(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeLessOrEqualTo(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().BeLessOrEqualTo(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().BeLessOrEqualTo(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().BeLessOrEqualTo(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().BeLessOrEqualTo(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountLessOrEqualTo_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveCountLessOrEqualTo_CountShouldBeLessOrEqualTo);

        [DataTestMethod]
        [AssertionDiagnostic("(actual.Count() + 1).Should().BeLessOrEqualTo(k{0});")]
        [AssertionDiagnostic("(actual.Count() + 1).Should().BeLessOrEqualTo(6{0});")]
        [Implemented]
        public void CollectionShouldHaveCountLessOrEqualTo_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeLessOrEqualTo(k{0});",
            newAssertion: "actual.Should().HaveCountLessOrEqualTo(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeLessOrEqualTo(6{0});",
            newAssertion: "actual.Should().HaveCountLessOrEqualTo(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeLessOrEqualTo(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountLessOrEqualTo(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeLessOrEqualTo(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountLessOrEqualTo(6{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count().Should().BeLessOrEqualTo(k{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().HaveCountLessOrEqualTo(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count().Should().BeLessOrEqualTo(6{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().HaveCountLessOrEqualTo(6{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Count().Should().BeLessOrEqualTo(k{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveCountLessOrEqualTo(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Count().Should().BeLessOrEqualTo(6{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveCountLessOrEqualTo(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountLessOrEqualTo_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().NotBe(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().NotBe(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().NotBe(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().NotBe(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().NotBe(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().NotBe(6{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().NotBe(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().NotBe(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotHaveCount_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotHaveCount_CountShouldNotBe);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().NotBe(k{0});",
            newAssertion: "actual.Should().NotHaveCount(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().NotBe(6{0});",
            newAssertion: "actual.Should().NotHaveCount(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().NotBe(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotHaveCount(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().NotBe(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotHaveCount(6{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count().Should().NotBe(k{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotHaveCount(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count().Should().NotBe(6{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotHaveCount(6{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Count().Should().NotBe(k{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().NotHaveCount(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Count().Should().NotBe(6{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().NotHaveCount(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotHaveCount_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveCount(expected.Count(){0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().HaveCount(expected.Count(){0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Should().HaveCount(expected.Count(){0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Should().HaveCount(expected.Count(){0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveSameCount_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveSameCount_ShouldHaveCountOtherCollectionCount);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveCount(expected.Count() + 1{0});")]
        [AssertionDiagnostic("actual.Should().HaveCount(expected.Count() + unexpected.Count(){0});")]
        [AssertionDiagnostic("actual.Should().HaveCount(1 + expected.Count(){0});")]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/300")]
        public void CollectionShouldHaveSameCount_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveCount(expected.Count(){0});",
            newAssertion: "actual.Should().HaveSameCount(expected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveCount(expected.Count(){0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveSameCount(expected{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Should().HaveCount(expected.Count(){0}).And.ToString();",
            newAssertion: "actual.ToList().Should().HaveSameCount(expected{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Should().HaveCount(expected.Count(){0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveSameCount(expected{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveSameCount_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().NotBe(unexpected.Count(){0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().NotBe(unexpected.Count(){0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Count().Should().NotBe(unexpected.Count(){0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Count().Should().NotBe(unexpected.Count(){0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotHaveSameCount_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotHaveSameCount_CountShouldNotBeOtherCollectionCount);

        [DataTestMethod]
        [AssertionDiagnostic("(actual.Count() + 1).Should().NotBe(unexpected.Count(){0});")]
        [AssertionDiagnostic("actual.Count().Should().NotBe((unexpected.Count() + 1){0});")]
        [AssertionDiagnostic("actual.Count().ToString().Length.Should().NotBe(unexpected.Count(){0});")]
        [Implemented]
        public void CollectionShouldNotHaveSameCount_TestNotAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().NotBe(unexpected.Count(){0});",
            newAssertion: "actual.Should().NotHaveSameCount(unexpected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().NotBe(unexpected.Count(){0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotHaveSameCount(unexpected{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Count().Should().NotBe(unexpected.Count(){0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotHaveSameCount(unexpected{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Count().Should().NotBe(unexpected.Count(){0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().NotHaveSameCount(unexpected{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotHaveSameCount_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveCount(1{0});")]
        [AssertionDiagnostic("actual.Should().HaveCount(1{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainSingle_TestAnalyzer_GenericIEnumerableShouldReport(string assertion)
        {
            var source = GenerateCode.GenericIEnumerableAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new LegacyDiagnosticResult
            {
                Id = FluentAssertionsAnalyzer.DiagnosticId,
                Message = DiagnosticMetadata.CollectionShouldContainSingle_ShouldHaveCount1.Message,
                VisitorName = DiagnosticMetadata.CollectionShouldContainSingle_ShouldHaveCount1.Name,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 11,13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveCount(1{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().HaveCount(1{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Should().HaveCount(1{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Should().HaveCount(1{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainSingle_ShouldHaveCount1_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldContainSingle_ShouldHaveCount1);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Where(x => x.BooleanProperty).Should().HaveCount(1{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Where(x => x.BooleanProperty).Should().HaveCount(1{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Where(x => x.BooleanProperty).Should().HaveCount(1{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Where(x => x.BooleanProperty).Should().HaveCount(1{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainSingle_WhereShouldHaveCount1_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldContainSingle_WhereShouldHaveCount1);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Where(x => x.BooleanProperty).Should().HaveCount(1{0});",
            newAssertion: "actual.Should().ContainSingle(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveCount(1{0});",
            newAssertion: "actual.Should().ContainSingle({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Where(x => x.BooleanProperty).Should().HaveCount(1{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().ContainSingle(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveCount(1{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().ContainSingle({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Where(x => x.BooleanProperty).Should().HaveCount(1{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().ContainSingle(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Should().HaveCount(1{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().ContainSingle({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveCount(1{0}).And.Contain(item => item == expectedItem);",
            newAssertion: "actual.Should().ContainSingle({0}).And.Contain(item => item == expectedItem);")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveCount(1{0}).And.Contain(item => item == expectedItem).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().ContainSingle({0}).And.Contain(item => item == expectedItem).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainSingle_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().NotBeEmpty().And.NotBeNull({0});")]
        [AssertionDiagnostic("actual.Should().NotBeEmpty({0}).And.NotBeNull();")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().NotBeEmpty().And.NotBeNull({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Should().NotBeEmpty().And.NotBeNull({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Should().NotBeEmpty().And.NotBeNull({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotBeNullOrEmpty_ShouldNotBeEmptyAndNotBeNull_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotBeNullOrEmpty_ShouldNotBeEmptyAndNotBeNull);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().NotBeNull().And.NotBeEmpty({0});")]
        [AssertionDiagnostic("actual.Should().NotBeNull({0}).And.NotBeEmpty();")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().NotBeNull().And.NotBeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Should().NotBeNull().And.NotBeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Should().NotBeNull().And.NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotBeNullOrEmpty_ShouldNotBeNullAndNotBeEmpty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotBeNullOrEmpty_ShouldNotBeNullAndNotBeEmpty);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeNull().And.NotBeEmpty({0});",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeEmpty().And.NotBeNull({0});",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeNull({0}).And.NotBeEmpty();",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeEmpty({0}).And.NotBeNull();",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().NotBeNull().And.NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotBeNullOrEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Should().NotBeEmpty().And.NotBeNull({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotBeNullOrEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Should().NotBeNull({0}).And.NotBeEmpty().And.ToString();",
            newAssertion: "actual.ToArray().Should().NotBeNullOrEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotBeNullOrEmpty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.ElementAt(k).Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.ElementAt(6).Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().ElementAt(k).Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().ElementAt(6).Should().Be(expectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveElementAt_ElementAtIndexShouldBe_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveElementAt_ElementAtIndexShouldBe);

        [DataTestMethod]
        [AssertionDiagnostic("actual.ElementAt(k).BooleanProperty.Should().Be(expectedItem.BooleanProperty{0});")]
        [AssertionDiagnostic("actual.ElementAt(6).BooleanProperty.Should().Be(expectedItem.BooleanProperty{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().ElementAt(k).BooleanProperty.Should().Be(expectedItem.BooleanProperty{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().ElementAt(6).BooleanProperty.Should().Be(expectedItem.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveElementAt_ElementAtIndexShouldBe_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionDiagnostic("actual[k].Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual[6].Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.ToArray()[k].Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.ToArray()[6].Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.ToList()[k].Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.ToList()[6].Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual[k].Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual[6].Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray()[k].Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray()[6].Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList()[k].Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList()[6].Should().Be(expectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveElementAt_IndexerShouldBe_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveElementAt_IndexerShouldBe);

        [DataTestMethod]
        [AssertionDiagnostic("var first = actual[0]; first[6].Should().Be(expectedItem{0});")]
        [Implemented]
        public void CollectionShouldHaveElementAt_IndexerShouldBe_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionDiagnostic("actual.Skip(k).First().Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.Skip(6).First().Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Skip(k).First().Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Skip(6).First().Should().Be(expectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveElementAt_SkipFirstShouldBe_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldHaveElementAt_SkipFirstShouldBe);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.ElementAt(k).Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(k, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ElementAt(6).Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(6, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual[k].Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(k, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual[6].Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(6, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Skip(k).First().Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(k, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Skip(6).First().Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(6, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().ElementAt(k).Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(k, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().ElementAt(6).Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(6, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray()[k].Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveElementAt(k, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray()[6].Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveElementAt(6, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList()[k].Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().HaveElementAt(k, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList()[6].Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().HaveElementAt(6, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Skip(k).First().Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(k, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Skip(6).First().Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(6, expectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveElementAt_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.OrderBy(x => x.BooleanProperty).Should().Equal(actual{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().OrderBy(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().OrderBy(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().OrderBy(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeInAscendingOrder_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldBeInAscendingOrder_OrderByShouldEqual);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.OrderBy(x => x.BooleanProperty).Should().Equal(actual{0});",
            newAssertion: "actual.Should().BeInAscendingOrder(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().OrderBy(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeInAscendingOrder(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().OrderBy(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().BeInAscendingOrder(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().OrderBy(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().BeInAscendingOrder(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeInAscendingOrder_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeInDescendingOrder_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldBeInDescendingOrder_OrderByDescendingShouldEqual);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0});",
            newAssertion: "actual.Should().BeInDescendingOrder(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeInDescendingOrder(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();",
            newAssertion: "actual.ToList().Should().BeInDescendingOrder(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().BeInDescendingOrder(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeInDescendingOrder_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldEqualOtherCollectionByComparer_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldEqualOtherCollectionByComparer_SelectShouldEqualOtherCollectionSelect);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0});",
            newAssertion: "actual.Should().Equal(expected, (e1, e2) => e1.BooleanProperty == e2.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().Equal(expected, (e1, e2) => e1.BooleanProperty == e2.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0}).And.ToString();",
            newAssertion: "actual.ToList().Should().Equal(expected, (e1, e2) => e1.BooleanProperty == e2.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().Equal(expected, (e1, e2) => e1.BooleanProperty == e2.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldEqualOtherCollectionByComparer_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Intersect(expected).Should().BeEmpty({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Intersect(expected).Should().BeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Intersect(expected).Should().BeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Intersect(expected).Should().BeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotIntersectWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotIntersectWith_IntersectShouldBeEmpty);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Intersect(expected).Should().BeEmpty({0});",
            newAssertion: "actual.Should().NotIntersectWith(expected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Intersect(expected).Should().BeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotIntersectWith(expected{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Intersect(expected).Should().BeEmpty({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().NotIntersectWith(expected{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Intersect(expected).Should().BeEmpty({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().NotIntersectWith(expected{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotIntersectWith_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Intersect(expected).Should().NotBeEmpty({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Intersect(expected).Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Intersect(expected).Should().NotBeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Intersect(expected).Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldIntersectWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldIntersectWith_IntersectShouldNotBeEmpty);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Intersect(expected).Should().NotBeEmpty({0});",
            newAssertion: "actual.Should().IntersectWith(expected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Intersect(expected).Should().NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().IntersectWith(expected{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Intersect(expected).Should().NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().IntersectWith(expected{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Intersect(expected).Should().NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().IntersectWith(expected{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldIntersectWith_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Select(x => x.BooleanProperty).Should().NotContainNulls({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Select(x => x.BooleanProperty).Should().NotContainNulls({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainNulls_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldNotContainNulls_SelectShouldNotContainNulls);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Select(x => x.BooleanProperty).Should().NotContainNulls({0});",
            newAssertion: "actual.Should().NotContainNulls(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Select(x => x.BooleanProperty).Should().NotContainNulls({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContainNulls(x => x.BooleanProperty{0}).And.ToString();")]
        [Ignore("What Should Happen?")]
        public void CollectionShouldNotContainNulls_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveSameCount(actual.Distinct(){0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().HaveSameCount(actual.AsEnumerable().Distinct(){0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Should().HaveSameCount(actual.ToList().Distinct(){0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Should().HaveSameCount(actual.ToArray().Distinct(){0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyHaveUniqueItems_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldOnlyHaveUniqueItems_ShouldHaveSameCountThisCollectionDistinct);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveSameCount(expected.Distinct(){0});")]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/299")]
        public void CollectionShouldOnlyHaveUniqueItems_TestNoAnalyzer(string assertion) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(GenerateCode.GenericIListCodeBlockAssertion(assertion));

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveSameCount(actual.Distinct(){0});",
            newAssertion: "actual.Should().OnlyHaveUniqueItems({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveSameCount(actual.AsEnumerable().Distinct(){0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().OnlyHaveUniqueItems({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Should().HaveSameCount(actual.ToList().Distinct(){0}).And.ToString();",
            newAssertion: "actual.ToList().Should().OnlyHaveUniqueItems({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Should().HaveSameCount(actual.ToArray().Distinct(){0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().OnlyHaveUniqueItems({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyHaveUniqueItems_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToList().Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToArray().Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyHaveUniqueItemsByComparer_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, DiagnosticMetadata.CollectionShouldOnlyHaveUniqueItemsByComparer_SelectShouldOnlyHaveUniqueItems);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0});",
            newAssertion: "actual.Should().OnlyHaveUniqueItems(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().OnlyHaveUniqueItems(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToList().Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0}).And.ToString();",
            newAssertion: "actual.ToList().Should().OnlyHaveUniqueItems(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray().Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().OnlyHaveUniqueItems(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyHaveUniqueItemsByComparer_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.FirstOrDefault().Should().BeNull({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().FirstOrDefault().Should().BeNull({0}).And.ToString();")]
        [NotImplemented]
        [Ignore("What Should Happen?")]
        public void CollectionShouldHaveElementAt0Null_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock(assertion, null);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.FirstOrDefault().Should().BeNull({0});",
            newAssertion: "actual.Should().HaveElementAt(0, null{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().FirstOrDefault().Should().BeNull({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(0, null{0}).And.ToString();")]
        [NotImplemented]
        [Ignore("What Should Happen?")]
        public void CollectionShouldHaveElementAt0Null_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock(oldAssertion, newAssertion);

        private void VerifyCSharpDiagnosticCodeBlock(string sourceAssertion, DiagnosticMetadata metadata)
        {
            var source = GenerateCode.GenericIListCodeBlockAssertion(sourceAssertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new LegacyDiagnosticResult
            {
                Id = FluentAssertionsAnalyzer.DiagnosticId,
                Message = metadata.Message,
                VisitorName = metadata.Name,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 11,13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpDiagnosticExpressionBody(string sourceAssertion, DiagnosticMetadata metadata)
        {
            var source = GenerateCode.GenericIListExpressionBodyAssertion(sourceAssertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new LegacyDiagnosticResult
            {
                Id = FluentAssertionsAnalyzer.DiagnosticId,
                VisitorName = metadata.Name,
                Message = metadata.Message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 10,16)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyArrayCSharpDiagnosticCodeBlock(string sourceAssertion, DiagnosticMetadata metadata)
        {
            var source = GenerateCode.GenericArrayCodeBlockAssertion(sourceAssertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new LegacyDiagnosticResult
            {
                Id = FluentAssertionsAnalyzer.DiagnosticId,
                Message = metadata.Message,
                VisitorName = metadata.Name,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 11,13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyArrayCSharpFixCodeBlock(string oldSourceAssertion, string newSourceAssertion)
        {
            var oldSource = GenerateCode.GenericArrayCodeBlockAssertion(oldSourceAssertion);
            var newSource = GenerateCode.GenericArrayCodeBlockAssertion(newSourceAssertion);

            VerifyFix(oldSource, newSource);
        }

        private void VerifyCSharpFixCodeBlock(string oldSourceAssertion, string newSourceAssertion)
        {
            var oldSource = GenerateCode.GenericIListCodeBlockAssertion(oldSourceAssertion);
            var newSource = GenerateCode.GenericIListCodeBlockAssertion(newSourceAssertion);

            VerifyFix(oldSource, newSource);
        }

        private void VerifyCSharpFixExpressionBody(string oldSourceAssertion, string newSourceAssertion)
        {
            var oldSource = GenerateCode.GenericIListExpressionBodyAssertion(oldSourceAssertion);
            var newSource = GenerateCode.GenericIListExpressionBodyAssertion(newSourceAssertion);

            VerifyFix(oldSource, newSource);
        }

        private void VerifyFix(string oldSource, string newSource)
        {
            DiagnosticVerifier.VerifyFix(new CodeFixVerifierArguments()
                .WithCodeFixProvider<FluentAssertionsCodeFixProvider>()
                .WithDiagnosticAnalyzer<FluentAssertionsAnalyzer>()
                .WithSources(oldSource)
                .WithFixedSources(newSource)
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
            );
        }
    }
}
