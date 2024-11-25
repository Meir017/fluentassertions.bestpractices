using FluentAssertions.Analyzers.TestUtils;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class DictionaryTests
    {
        [DataTestMethod]
        [AssertionDiagnostic("actual.ContainsKey(expectedKey).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).ContainsKey(expectedKey).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainKey_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.DictionaryShouldContainKey_ContainsKeyShouldBeTrue);

        [DataTestMethod]
        [DataRow(
@"using System.Collections.Generic;
using FluentAssertions;

namespace TestNamespace
{
    public class MultiKeyDict<TKey1, TKey2, TValue> : Dictionary<TKey1, Dictionary<TKey2, TValue>>
    {
        public bool ContainsKey(TKey1 key1, TKey2 key2) => false;
        public bool ContainsValue(TKey1 key1, TKey2 key2) => false;
    }

    public class TestClass
    {
        public void TestMethod(MultiKeyDict<int, int, string> actual)
        {
            actual.ContainsKey(0, 1).Should().BeTrue();
            actual.ContainsValue(0, 1).Should().BeTrue();
        }
    }
}")]
        [Implemented]
        public void DictionaryMethods_CustomMethods_TestNoAnalyzer(string code) => DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(code);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.ContainsKey(expectedKey).Should().BeTrue({0});",
            newAssertion: "actual.Should().ContainKey(expectedKey{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).ContainsKey(expectedKey).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(expectedKey{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainKey_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.ContainsKey(expectedKey).Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).ContainsKey(expectedKey).Should().BeFalse({0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldNotContainKey_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.DictionaryShouldNotContainKey_ContainsKeyShouldBeFalse);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.ContainsKey(expectedKey).Should().BeFalse({0});",
            newAssertion: "actual.Should().NotContainKey(expectedKey{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).ContainsKey(expectedKey).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().NotContainKey(expectedKey{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldNotContainKey_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.ContainsValue(expectedValue).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).ContainsValue(expectedValue).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainValue_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.DictionaryShouldContainValue_ContainsValueShouldBeTrue);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.ContainsValue(expectedValue).Should().BeTrue({0});",
            newAssertion: "actual.Should().ContainValue(expectedValue{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).ContainsValue(expectedValue).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(expectedValue{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainValue_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.ContainsValue(expectedValue).Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).ContainsValue(expectedValue).Should().BeFalse({0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldNotContainValue_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.DictionaryShouldNotContainValue_ContainsValueShouldBeFalse);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.ContainsValue(expectedValue).Should().BeFalse({0});",
            newAssertion: "actual.Should().NotContainValue(expectedValue{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).ContainsValue(expectedValue).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().NotContainValue(expectedValue{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldNotContainValue_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().ContainKey(expectedKey{0}).And.ContainValue(expectedValue);")]
        [AssertionDiagnostic("actual.Should().ContainKey(expectedKey).And.ContainValue(expectedValue{0});")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(expectedKey{0}).And.ContainValue(expectedValue).And.ToString();")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(expectedKey).And.ContainValue(expectedValue{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainKeyAndValue_ShouldContainKeyAndContainValue_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.DictionaryShouldContainKeyAndValue_ShouldContainKeyAndContainValue);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().ContainValue(expectedValue{0}).And.ContainKey(expectedKey);")]
        [AssertionDiagnostic("actual.Should().ContainValue(expectedValue).And.ContainKey(expectedKey{0});")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(expectedValue{0}).And.ContainKey(expectedKey).And.ToString();")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(expectedValue).And.ContainKey(expectedKey{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainKeyAndValue_ShouldContainValueAndContainKey_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.DictionaryShouldContainKeyAndValue_ShouldContainValueAndContainKey);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().ContainKey(expectedKey{0}).And.ContainValue(expectedValue);",
            newAssertion: "actual.Should().Contain(expectedKey, expectedValue{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(expectedKey{0}).And.ContainValue(expectedValue).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().Contain(expectedKey, expectedValue{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().ContainKey(expectedKey).And.ContainValue(expectedValue{0});",
            newAssertion: "actual.Should().Contain(expectedKey, expectedValue{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(expectedKey).And.ContainValue(expectedValue{0}).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().Contain(expectedKey, expectedValue{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().ContainValue(expectedValue{0}).And.ContainKey(expectedKey);",
            newAssertion: "actual.Should().Contain(expectedKey, expectedValue{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(expectedValue).And.ContainKey(expectedKey{0}).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().Contain(expectedKey, expectedValue{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().ContainValue(expectedValue).And.ContainKey(expectedKey{0});",
            newAssertion: "actual.Should().Contain(expectedKey, expectedValue{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(expectedValue{0}).And.ContainKey(expectedKey).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().Contain(expectedKey, expectedValue{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainKeyAndValue_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().ContainKey(pair.Key{0}).And.ContainValue(pair.Value);")]
        [AssertionDiagnostic("actual.Should().ContainKey(pair.Key).And.ContainValue(pair.Value{0});")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(pair.Key{0}).And.ContainValue(pair.Value).And.ToString();")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(pair.Key).And.ContainValue(pair.Value{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainPair_ShouldContainKeyAndContainValue_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.DictionaryShouldContainPair_ShouldContainKeyAndContainValue);

        [DataTestMethod]
        [AssertionDiagnostic("actual.Should().ContainValue(pair.Value{0}).And.ContainKey(pair.Key);")]
        [AssertionDiagnostic("actual.Should().ContainValue(pair.Value).And.ContainKey(pair.Key{0});")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(pair.Value{0}).And.ContainKey(pair.Key).And.ToString();")]
        [AssertionDiagnostic("actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(pair.Value).And.ContainKey(pair.Key{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainPair_ShouldContainValueAndContainKey_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.DictionaryShouldContainPair_ShouldContainValueAndContainKey);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().ContainKey(pair.Key{0}).And.ContainValue(pair.Value);",
            newAssertion: "actual.Should().Contain(pair{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(pair.Key{0}).And.ContainValue(pair.Value).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().Contain(pair{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().ContainKey(pair.Key).And.ContainValue(pair.Value{0});",
            newAssertion: "actual.Should().Contain(pair{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainKey(pair.Key).And.ContainValue(pair.Value{0}).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().Contain(pair{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().ContainValue(pair.Value{0}).And.ContainKey(pair.Key);",
            newAssertion: "actual.Should().Contain(pair{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(pair.Value{0}).And.ContainKey(pair.Key).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().Contain(pair{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().ContainValue(pair.Value).And.ContainKey(pair.Key{0});",
            newAssertion: "actual.Should().Contain(pair{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().ContainValue(pair.Value).And.ContainKey(pair.Key{0}).And.ToString();",
            newAssertion: "actual.ToDictionary(p => p.Key, p=> p.Value).Should().Contain(pair{0}).And.ToString();")]
        [Implemented]
        public void DictionaryShouldContainPair_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        private void VerifyCSharpDiagnostic(string sourceAssersion, DiagnosticMetadata metadata)
        {
            var source = GenerateCode.GenericIDictionaryAssertion(sourceAssersion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new LegacyDiagnosticResult
            {
                Id = FluentAssertionsAnalyzer.DiagnosticId,
                Message = metadata.Message,
                VisitorName = metadata.Name,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 12,13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpFix(string oldSourceAssertion, string newSourceAssertion)
        {
            var oldSource = GenerateCode.GenericIDictionaryAssertion(oldSourceAssertion);
            var newSource = GenerateCode.GenericIDictionaryAssertion(newSourceAssertion);

            DiagnosticVerifier.VerifyFix(new CodeFixVerifierArguments()
                .WithSources(oldSource)
                .WithFixedSources(newSource)
                .WithDiagnosticAnalyzer<FluentAssertionsAnalyzer>()
                .WithCodeFixProvider<FluentAssertionsCodeFixProvider>()
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
            );
        }
    }
}