using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions.Analyzers.TestUtils;

namespace FluentAssertions.Analyzers.Tests.Tips
{
    [TestClass]
    public class XunitTests
    {
        [TestMethod]
        [Implemented]
        public void SupportExcludingMethods()
        {
            var source = GenerateCode.XunitAssertion("bool actual", "Assert.True(actual);");
            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithAllAnalyzers()
                .WithSources(source)
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, PackageReference.XunitAssert_2_5_1)
                .WithAnalyzerConfigOption("ffa_excluded_methods", "M:Xunit.Assert.True(System.Boolean)")
                .WithExpectedDiagnostics()
            );
        }

        [DataTestMethod]
        [DataRow("Assert.True(actual);")]
        [DataRow("Assert.True(actual, \"because it's possible\");")]
        [DataRow("Assert.True(bool.Parse(\"true\"));")]
        [DataRow("Assert.True(bool.Parse(\"true\"), \"because it's possible\");")]
        [Implemented]
        public void AssertTrue_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic("bool actual", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.True(actual);",
            /* newAssertion: */ "actual.Should().BeTrue();")]
        [DataRow(
            /* oldAssertion: */ "Assert.True(actual, \"because it's possible\");",
            /* newAssertion: */ "actual.Should().BeTrue(\"because it's possible\");")]
        [DataRow(
            /* oldAssertion: */ "Assert.True(bool.Parse(\"true\"));",
            /* newAssertion: */ "bool.Parse(\"true\").Should().BeTrue();")]
        [DataRow(
            /* oldAssertion: */ "Assert.True(bool.Parse(\"true\"), \"because it's possible\");",
            /* newAssertion: */ "bool.Parse(\"true\").Should().BeTrue(\"because it's possible\");")]
        [DataRow(
            /* oldAssertion: */ "Assert.True(!actual);",
            /* newAssertion: */ "(!actual).Should().BeTrue();")]
        [DataRow(
            /* oldAssertion: */ "Assert.True(actual == false);",
            /* newAssertion: */ "(actual == false).Should().BeTrue();")]
        [Implemented]
        public void AssertTrue_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("bool actual", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.False(actual);")]
        [DataRow("Assert.False(actual, \"because it's possible\");")]
        [DataRow("Assert.False(bool.Parse(\"false\"));")]
        [DataRow("Assert.False(bool.Parse(\"false\"), \"because it's possible\");")]
        [Implemented]
        public void AssertFalse_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic("bool actual", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.False(actual);",
            /* newAssertion: */ "actual.Should().BeFalse();")]
        [DataRow(
            /* oldAssertion: */ "Assert.False(actual, \"because it's possible\");",
            /* newAssertion: */ "actual.Should().BeFalse(\"because it's possible\");")]
        [DataRow(
            /* oldAssertion: */ "Assert.False(bool.Parse(\"false\"));",
            /* newAssertion: */ "bool.Parse(\"false\").Should().BeFalse();")]
        [DataRow(
            /* oldAssertion: */ "Assert.False(bool.Parse(\"false\"), \"because it's possible\");",
            /* newAssertion: */ "bool.Parse(\"false\").Should().BeFalse(\"because it's possible\");")]
        [Implemented]
        public void AssertFalse_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("bool actual", oldAssertion, newAssertion);


        [DataTestMethod]
        [DataRow("Assert.Same(expected, actual);")]
        [Implemented]
        public void AssertSame_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic("object actual, object expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Same(expected, actual);",
            /* newAssertion: */ "actual.Should().BeSameAs(expected);")]
        [Implemented]
        public void AssertSame_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.NotSame(expected, actual);")]
        [Implemented]
        public void AssertNotSame_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic("object actual, object expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.NotSame(expected, actual);",
            /* newAssertion: */ "actual.Should().NotBeSameAs(expected);")]
        [Implemented]
        public void AssertNotSame_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Equal(expected, actual, tolerance);")]
        [DataRow("Assert.Equal(expected, actual, 0.6f);")]
        [Implemented]
        public void AssertFloatEqualWithTolerance_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("float actual, float expected, float tolerance", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, tolerance);",
            /* newAssertion: */ "actual.Should().BeApproximately(expected, tolerance);")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, 0.6f);",
            /* newAssertion: */ "actual.Should().BeApproximately(expected, 0.6f);")]
        [Implemented]
        public void AssertFloatEqualWithTolerance_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("float actual, float expected, float tolerance", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Equal(expected, actual, tolerance);")]
        [DataRow("Assert.Equal(expected, actual, 0.6);")]
        [Implemented]
        public void AssertDoubleEqualWithTolerance_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("double actual, double expected, double tolerance", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, tolerance);",
            /* newAssertion: */ "actual.Should().BeApproximately(expected, tolerance);")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, 0.6);",
            /* newAssertion: */ "actual.Should().BeApproximately(expected, 0.6);")]
        [Implemented]
        public void AssertDoubleEqualWithTolerance_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("double actual, double expected, double tolerance", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Equal(expected, actual, precision);")]
        [DataRow("Assert.Equal(expected, actual, 5);")]
        [Implemented]
        public void AssertDoubleEqualWithPrecision_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("double actual, double expected, int precision", assertion);

        // There is no corresponding API in FluentAssertions
        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, precision);",
            /* newAssertion: */ "Assert.Equal(expected, actual, precision);")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, 5);",
            /* newAssertion: */ "Assert.Equal(expected, actual, 5);")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, precision, rounding);",
            /* newAssertion: */ "Assert.Equal(expected, actual, precision, rounding);")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, 5, MidpointRounding.ToEven);",
            /* newAssertion: */ "Assert.Equal(expected, actual, 5, MidpointRounding.ToEven);")]
        [Implemented]
        public void AssertDoubleEqualWithPrecision_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("double actual, double expected, int precision, MidpointRounding rounding", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Equal(expected, actual, precision);")]
        [DataRow("Assert.Equal(expected, actual, 5);")]
        [Implemented]
        public void AssertDecimalEqualWithPrecision_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("decimal actual, decimal expected, int precision", assertion);

        // There is no corresponding API in FluentAssertions
        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, precision);",
            /* newAssertion: */ "Assert.Equal(expected, actual, precision);")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, 5);",
            /* newAssertion: */ "Assert.Equal(expected, actual, 5);")]
        [Implemented]
        public void AssertDecimalEqualWithPrecision_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("decimal actual, decimal expected, int precision", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Equal(expected, actual, precision);")]
        [DataRow("Assert.Equal(expected, actual, TimeSpan.FromSeconds(1));")]
        [Implemented]
        public void AssertDateTimeEqual_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic("DateTime actual, DateTime expected, TimeSpan precision", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, precision);",
            /* newAssertion: */ "actual.Should().BeCloseTo(expected, precision);")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, TimeSpan.FromSeconds(1));",
            /* newAssertion: */ "actual.Should().BeCloseTo(expected, TimeSpan.FromSeconds(1));")]
        [Implemented]
        public void AssertDateTimeEqual_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("DateTime actual, DateTime expected, TimeSpan precision", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Equal(expected, actual, comparer);")]
        [DataRow("Assert.Equal(expected, actual, ReferenceEqualityComparer.Instance);")]
        [Implemented]
        public void AssertObjectEqualWithComparer_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("object actual, object expected, IEqualityComparer<object> comparer", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, comparer);",
            /* newAssertion: */ "actual.Should().BeEquivalentTo(expected, options => options.Using(comparer));")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual, ReferenceEqualityComparer.Instance);",
            /* newAssertion: */ "actual.Should().BeEquivalentTo(expected, options => options.Using(ReferenceEqualityComparer.Instance));")]
        [Implemented]
        public void AssertObjectEqualWithComparer_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected, IEqualityComparer<object> comparer", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Equal(expected, actual);")]
        [Implemented]
        public void AssertObjectEqual_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic("object actual, object expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual);",
            /* newAssertion: */ "actual.Should().Be(expected);")]
        [Implemented]
        public void AssertObjectEqual_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Equal(expected, actual);")]
        [Implemented]
        public void AssertStringEqual_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, string expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equal(expected, actual);",
            /* newAssertion: */ "actual.Should().Be(expected);")]
        [Implemented]
        public void AssertStringEqual_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, string expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.StrictEqual(expected, actual);")]
        [Implemented]
        public void AssertStrictEqual_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("object actual, object expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.StrictEqual(expected, actual);",
            /* newAssertion: */ "actual.Should().Be(expected);")]
        [Implemented]
        public void AssertStrictEqual_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.NotEqual(expected, actual, precision);")]
        [DataRow("Assert.NotEqual(expected, actual, 5);")]
        [Implemented]
        public void AssertDoubleNotEqualWithPrecision_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("double actual, double expected, int precision", assertion);

        // There is no corresponding API in FluentAssertions
        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.NotEqual(expected, actual, precision);",
            /* newAssertion: */ "Assert.NotEqual(expected, actual, precision);")]
        [DataRow(
            /* oldAssertion: */ "Assert.NotEqual(expected, actual, 5);",
            /* newAssertion: */ "Assert.NotEqual(expected, actual, 5);")]
        [Implemented]
        public void AssertDoubleNotEqualWithPrecision_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("double actual, double expected, int precision", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.NotEqual(expected, actual, precision);")]
        [DataRow("Assert.NotEqual(expected, actual, 5);")]
        [Implemented]
        public void AssertDecimalNotEqualWithPrecision_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("decimal actual, decimal expected, int precision", assertion);

        // There is no corresponding API in FluentAssertions
        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.NotEqual(expected, actual, precision);",
            /* newAssertion: */ "Assert.NotEqual(expected, actual, precision);")]
        [DataRow(
            /* oldAssertion: */ "Assert.NotEqual(expected, actual, 5);",
            /* newAssertion: */ "Assert.NotEqual(expected, actual, 5);")]
        [Implemented]
        public void AssertDecimalNotEqualWithPrecision_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("decimal actual, decimal expected, int precision", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.NotEqual(expected, actual, comparer);")]
        [DataRow("Assert.NotEqual(expected, actual, ReferenceEqualityComparer.Instance);")]
        [Implemented]
        public void AssertObjectNotEqualWithComparer_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("object actual, object expected, IEqualityComparer<object> comparer", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.NotEqual(expected, actual, comparer);",
            /* newAssertion: */ "actual.Should().NotBeEquivalentTo(expected, options => options.Using(comparer));")]
        [DataRow(
            /* oldAssertion: */ "Assert.NotEqual(expected, actual, ReferenceEqualityComparer.Instance);",
            /* newAssertion: */ "actual.Should().NotBeEquivalentTo(expected, options => options.Using(ReferenceEqualityComparer.Instance));")]
        [Implemented]
        public void AssertObjectNotEqualWithComparer_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected, IEqualityComparer<object> comparer", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.NotEqual(expected, actual);")]
        [Implemented]
        public void AssertObjectNotEqual_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("object actual, object expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.NotEqual(expected, actual);",
            /* newAssertion: */ "actual.Should().NotBe(expected);")]
        [Implemented]
        public void AssertObjectNotEqual_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.NotStrictEqual(expected, actual);")]
        [Implemented]
        public void AssertObjectNotStrictEqual_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("object actual, object expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.NotStrictEqual(expected, actual);",
            /* newAssertion: */ "actual.Should().NotBe(expected);")]
        [Implemented]
        public void AssertObjectNotStrictEqual_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Null(actual);")]
        [Implemented]
        public void AssertNull_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("object actual", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Null(actual);",
            /* newAssertion: */ "actual.Should().BeNull();")]
        [Implemented]
        public void AssertNull_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.NotNull(actual);")]
        [Implemented]
        public void AssertNotNull_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("object actual", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.NotNull(actual);",
            /* newAssertion: */ "actual.Should().NotBeNull();")]
        [Implemented]
        public void AssertNotNull_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Contains(expected, actual);")]
        [Implemented]
        public void AssertStringContains_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, string expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Contains(expected, actual);",
            /* newAssertion: */ "actual.Should().Contain(expected);")]
        [Implemented]
        public void AssertStringContains_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, string expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Contains(expected, actual);", "ISet<string> actual, string expected")]
        [DataRow("Assert.Contains(expected, actual);", "IReadOnlySet<string> actual, string expected")]
        [DataRow("Assert.Contains(expected, actual);", "HashSet<string> actual, string expected")]
        [DataRow("Assert.Contains(expected, actual);", "ImmutableHashSet<string> actual, string expected")]
        [Implemented]
        public void AssertSetContains_TestAnalyzer(string assertion, string arguments) =>
            VerifyCSharpDiagnostic(arguments, assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Contains(expected, actual);",
            /* newAssertion: */ "actual.Should().Contain(expected);",
            /* arguments: */ "ISet<string> actual, string expected")]
        [DataRow(
            /* oldAssertion: */ "Assert.Contains(expected, actual);",
            /* newAssertion: */ "actual.Should().Contain(expected);",
            /* arguments: */ "IReadOnlySet<string> actual, string expected")]
        [DataRow(
            /* oldAssertion: */ "Assert.Contains(expected, actual);",
            /* newAssertion: */ "actual.Should().Contain(expected);",
            /* arguments: */ "HashSet<string> actual, string expected")]
        [DataRow(
            /* oldAssertion: */ "Assert.Contains(expected, actual);",
            /* newAssertion: */ "actual.Should().Contain(expected);",
            /* arguments: */ "ImmutableHashSet<string> actual, string expected")]
        [Implemented]
        public void AssertSetContains_TestCodeFix(string oldAssertion, string newAssertion, string arguments)
            => VerifyCSharpFix(arguments, oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.DoesNotContain(expected, actual);")]
        [Implemented]
        public void AssertStringDoesNotContain_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, string expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.DoesNotContain(expected, actual);",
            /* newAssertion: */ "actual.Should().NotContain(expected);")]
        [Implemented]
        public void AssertStringDoesNotContain_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, string expected", oldAssertion, newAssertion);


        [DataTestMethod]
        [DataRow("Assert.DoesNotContain(expected, actual);", "ISet<string> actual, string expected")]
        [DataRow("Assert.DoesNotContain(expected, actual);", "IReadOnlySet<string> actual, string expected")]
        [DataRow("Assert.DoesNotContain(expected, actual);", "HashSet<string> actual, string expected")]
        [DataRow("Assert.DoesNotContain(expected, actual);", "ImmutableHashSet<string> actual, string expected")]
        [Implemented]
        public void AssertSetDoesNotContain_TestAnalyzer(string assertion, string arguments) =>
            VerifyCSharpDiagnostic(arguments, assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.DoesNotContain(expected, actual);",
            /* newAssertion: */ "actual.Should().NotContain(expected);",
            /* arguments: */ "ISet<string> actual, string expected")]
        [DataRow(
            /* oldAssertion: */ "Assert.DoesNotContain(expected, actual);",
            /* newAssertion: */ "actual.Should().NotContain(expected);",
            /* arguments: */ "IReadOnlySet<string> actual, string expected")]
        [DataRow(
            /* oldAssertion: */ "Assert.DoesNotContain(expected, actual);",
            /* newAssertion: */ "actual.Should().NotContain(expected);",
            /* arguments: */ "HashSet<string> actual, string expected")]
        [DataRow(
            /* oldAssertion: */ "Assert.DoesNotContain(expected, actual);",
            /* newAssertion: */ "actual.Should().NotContain(expected);",
            /* arguments: */ "ImmutableHashSet<string> actual, string expected")]
        [Implemented]
        public void AssertSetDoesNotContain_TestCodeFix(string oldAssertion, string newAssertion, string arguments)
            => VerifyCSharpFix(arguments, oldAssertion, newAssertion);


        [DataTestMethod]
        [DataRow("Assert.Matches(expectedRegexPattern, actual);")]
        [Implemented]
        public void AssertStringMatches_String_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, string expectedRegexPattern", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Matches(expectedRegexPattern, actual);",
            /* newAssertion: */ "actual.Should().MatchRegex(expectedRegexPattern);")]
        [Implemented]
        public void AssertStringMatches_String_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, string expectedRegexPattern", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Matches(expectedRegex, actual);")]
        [Implemented]
        public void AssertStringMatches_Regex_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, Regex expectedRegex", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Matches(expectedRegex, actual);",
            /* newAssertion: */ "actual.Should().MatchRegex(expectedRegex);")]
        [Implemented]
        public void AssertStringMatches_Regex_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, Regex expectedRegex", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.DoesNotMatch(expectedRegexPattern, actual);")]
        [Implemented]
        public void AssertStringDoesNotMatch_String_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, string expectedRegexPattern", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.DoesNotMatch(expectedRegexPattern, actual);",
            /* newAssertion: */ "actual.Should().NotMatchRegex(expectedRegexPattern);")]
        [Implemented]
        public void AssertStringDoesNotMatch_String_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, string expectedRegexPattern", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.DoesNotMatch(expectedRegex, actual);")]
        [Implemented]
        public void AssertStringDoesNotMatch_Regex_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, Regex expectedRegex", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.DoesNotMatch(expectedRegex, actual);",
            /* newAssertion: */ "actual.Should().NotMatchRegex(expectedRegex);")]
        [Implemented]
        public void AssertStringDoesNotMatch_Regex_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, Regex expectedRegex", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Empty(actual);")]
        [Implemented]
        public void AssertEmpty_String_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Empty(actual);",
            /* newAssertion: */ "actual.Should().BeEmpty();")]
        [Implemented]
        public void AssertEmpty_String_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.EndsWith(expected, actual);")]
        [Implemented]
        public void AssertEndsWith_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, string expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.EndsWith(expected, actual);",
            /* newAssertion: */ "actual.Should().EndWith(expected);")]
        [Implemented]
        public void AssertEndsWith_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, string expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.StartsWith(expected, actual);")]
        [Implemented]
        public void AssertStartsWith_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, string expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.StartsWith(expected, actual);",
            /* newAssertion: */ "actual.Should().StartWith(expected);")]
        [Implemented]
        public void AssertStartsWith_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, string expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.Subset(expected, actual);")]
        [Implemented]
        public void AssertSubset_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("ISet<string> actual, ISet<string> expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Subset(expected, actual);",
            /* newAssertion: */ "actual.Should().BeSubsetOf(expected);")]
        [Implemented]
        public void AssertSubset_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("ISet<string> actual, ISet<string> expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.IsAssignableFrom(expected, actual);")]
        [DataRow("Assert.IsAssignableFrom(typeof(string), actual);")]
        [DataRow("Assert.IsAssignableFrom<string>(actual);")]
        [Implemented]
        public void AssertIsAssignableFrom_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, Type expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.IsAssignableFrom(expected, actual);",
            /* newAssertion: */ "actual.Should().BeAssignableTo(expected);")]
        [DataRow(
            /* oldAssertion: */ "Assert.IsAssignableFrom(typeof(string), actual);",
            /* newAssertion: */ "actual.Should().BeAssignableTo<string>();")]
        [DataRow(
            /* oldAssertion: */ "Assert.IsAssignableFrom<string>(actual);",
            /* newAssertion: */ "actual.Should().BeAssignableTo<string>();")]
        [Implemented]
        public void AssertIsAssignableFrom_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, Type expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.IsNotAssignableFrom(expected, actual);")]
        [DataRow("Assert.IsNotAssignableFrom(typeof(string), actual);")]
        [DataRow("Assert.IsNotAssignableFrom<string>(actual);")]
        [Implemented]
        public void AssertIsNotAssignableFrom_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, Type expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.IsNotAssignableFrom(expected, actual);",
            /* newAssertion: */ "actual.Should().NotBeAssignableTo(expected);")]
        [DataRow(
            /* oldAssertion: */ "Assert.IsNotAssignableFrom(typeof(string), actual);",
            /* newAssertion: */ "actual.Should().NotBeAssignableTo<string>();")]
        [DataRow(
            /* oldAssertion: */ "Assert.IsNotAssignableFrom<string>(actual);",
            /* newAssertion: */ "actual.Should().NotBeAssignableTo<string>();")]
        [Implemented]
        public void AssertIsNotAssignableFrom_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, Type expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.IsType(expected, actual);")]
        [DataRow("Assert.IsType(typeof(string), actual);")]
        [DataRow("Assert.IsType<string>(actual);")]
        [Implemented]
        public void AssertIsType_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, Type expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.IsType(expected, actual);",
            /* newAssertion: */ "actual.Should().BeOfType(expected);")]
        [DataRow(
            /* oldAssertion: */ "Assert.IsType(typeof(string), actual);",
            /* newAssertion: */ "actual.Should().BeOfType<string>();")]
        [DataRow(
            /* oldAssertion: */ "Assert.IsType<string>(actual);",
            /* newAssertion: */ "actual.Should().BeOfType<string>();")]
        [Implemented]
        public void AssertIsType_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, Type expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.IsNotType(expected, actual);")]
        [DataRow("Assert.IsNotType(typeof(string), actual);")]
        [DataRow("Assert.IsNotType<string>(actual);")]
        [Implemented]
        public void AssertIsNotType_TestAnalyzer(string assertion) =>
            VerifyCSharpDiagnostic("string actual, Type expected", assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.IsNotType(expected, actual);",
            /* newAssertion: */ "actual.Should().NotBeOfType(expected);")]
        [DataRow(
            /* oldAssertion: */ "Assert.IsNotType(typeof(string), actual);",
            /* newAssertion: */ "actual.Should().NotBeOfType<string>();")]
        [DataRow(
            /* oldAssertion: */ "Assert.IsNotType<string>(actual);",
            /* newAssertion: */ "actual.Should().NotBeOfType<string>();")]
        [Implemented]
        public void AssertIsNotType_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("string actual, Type expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Assert.InRange(actual, low, high);")]
        [Implemented]
        public void AssertInRange_TestAnalyzer(string assertion)
        {
            VerifyCSharpDiagnostic("double actual, double low, double high", assertion);
            VerifyCSharpDiagnostic("float actual, float low, float high", assertion);
            VerifyCSharpDiagnostic("int actual, int low, int high", assertion);
            VerifyCSharpDiagnostic("long actual, long low, long high", assertion);
        }

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.InRange(actual, low, high);",
            /* newAssertion: */ "actual.Should().BeInRange(low, high);")]
        [Implemented]
        public void AssertInRange_TestCodeFix(string oldAssertion, string newAssertion)
        {
            VerifyCSharpFix("double actual, double low, double high", oldAssertion, newAssertion);
            VerifyCSharpFix("float actual, float low, float high", oldAssertion, newAssertion);
            VerifyCSharpFix("int actual, int low, int high", oldAssertion, newAssertion);
            VerifyCSharpFix("long actual, long low, long high", oldAssertion, newAssertion);
        }

        [DataTestMethod]
        [DataRow("object actual, object expected", "Assert.Equivalent(expected, actual);")]
        [DataRow("object actual, object expected", "Assert.Equivalent(expected, actual, false);")]
        [DataRow("DateTime actual, DateTime expected", "Assert.Equivalent(expected, actual);")]
        [DataRow("DateTime actual, DateTime expected", "Assert.Equivalent(expected, actual, false);")]
        [DataRow("int actual, int expected", "Assert.Equivalent(expected, actual);")]
        [DataRow("int actual, int expected", "Assert.Equivalent(expected, actual, false);")]
        [Implemented]
        public void AssertEquivalent_TestAnalyzer(string arguments, string assertion) =>
            VerifyCSharpDiagnostic(arguments, assertion);

        [DataTestMethod]
        [DataRow(
            /* oldAssertion: */ "Assert.Equivalent(expected, actual);",
            /* newAssertion: */ "actual.Should().BeEquivalentTo(expected);")]
        [DataRow(
            /* oldAssertion: */ "Assert.Equivalent(expected, actual, false);",
            /* newAssertion: */ "actual.Should().BeEquivalentTo(expected);")]
        [Implemented]
        public void AssertEquivalent_TestCodeFix(string oldAssertion, string newAssertion)
            => VerifyCSharpFix("object actual, object expected", oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Action action", "Assert.Throws(typeof(ArgumentException), action);")]
        [DataRow("Action action, Type exceptionType", "Assert.Throws(exceptionType, action);")]
        [DataRow("Action action", "Assert.Throws<NullReferenceException>(action);")]
        [DataRow("Action action", "Assert.Throws<ArgumentException>(\"propertyName\", action);")]
        [Implemented]
        public void AssertThrows_TestAnalyzer(string arguments, string assertion)
            => VerifyCSharpDiagnostic(arguments, assertion);

        [DataTestMethod]
        [DataRow("Action action",
            /* oldAssertion */ "Assert.Throws(typeof(ArgumentException), action);",
            /* newAssertion */ "action.Should().ThrowExactly<ArgumentException>();")]
        [DataRow("Action action",
            /* oldAssertion */ "Assert.Throws<NullReferenceException>(action);",
            /* newAssertion */ "action.Should().ThrowExactly<NullReferenceException>();")]
        [DataRow("Action action",
            /* oldAssertion */ "Assert.Throws<ArgumentException>(\"propertyName\", action);",
            /* newAssertion */ "action.Should().ThrowExactly<ArgumentException>().WithParameterName(\"propertyName\");")]
        [Implemented]
        public void AssertThrows_TestCodeFix(string arguments, string oldAssertion, string newAssertion)
            => VerifyCSharpFix(arguments, oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Func<Task> action", "Assert.ThrowsAsync(typeof(ArgumentException), action);")]
        [DataRow("Func<Task> action, Type exceptionType", "Assert.ThrowsAsync(exceptionType, action);")]
        [DataRow("Func<Task> action", "Assert.ThrowsAsync<NullReferenceException>(action);")]
        [DataRow("Func<Task> action", "Assert.ThrowsAsync<ArgumentException>(\"propertyName\", action);")]
        [Implemented]
        public void AssertThrowsAsync_TestAnalyzer(string arguments, string assertion)
            => VerifyCSharpDiagnostic(arguments, assertion);

        [DataTestMethod]
        [DataRow("Func<Task> action",
            /* oldAssertion */ "Assert.ThrowsAsync(typeof(ArgumentException), action);",
            /* newAssertion */ "action.Should().ThrowExactlyAsync<ArgumentException>();")]
        [DataRow("Func<Task> action",
            /* oldAssertion */ "Assert.ThrowsAsync<NullReferenceException>(action);",
            /* newAssertion */ "action.Should().ThrowExactlyAsync<NullReferenceException>();")]
        [DataRow("Func<Task> action",
            /* oldAssertion */ "Assert.ThrowsAsync<ArgumentException>(\"propertyName\", action);",
            /* newAssertion */ "action.Should().ThrowExactlyAsync<ArgumentException>().WithParameterName(\"propertyName\");")]
        [Implemented]
        public void AssertThrowsAsync_TestCodeFix(string arguments, string oldAssertion, string newAssertion)
            => VerifyCSharpFix(arguments, oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Action action", "Assert.ThrowsAny<NullReferenceException>(action);")]
        [Implemented]
        public void AssertThrowsAny_TestAnalyzer(string arguments, string assertion)
            => VerifyCSharpDiagnostic(arguments, assertion);

        [DataTestMethod]
        [DataRow("Action action",
            /* oldAssertion */ "Assert.ThrowsAny<NullReferenceException>(action);",
            /* newAssertion */ "action.Should().Throw<NullReferenceException>();")]
        [Implemented]
        public void AssertThrowsAny_TestCodeFix(string arguments, string oldAssertion, string newAssertion)
            => VerifyCSharpFix(arguments, oldAssertion, newAssertion);

        [DataTestMethod]
        [DataRow("Func<Task> action", "Assert.ThrowsAnyAsync<NullReferenceException>(action);")]
        [Implemented]
        public void AssertThrowsAnyAsync_TestAnalyzer(string arguments, string assertion)
            => VerifyCSharpDiagnostic(arguments, assertion);

        [DataTestMethod]
        [DataRow("Func<Task> action",
            /* oldAssertion */ "Assert.ThrowsAnyAsync<NullReferenceException>(action);",
            /* newAssertion */ "action.Should().ThrowAsync<NullReferenceException>();")]
        [Implemented]
        public void AssertThrowsAnyAsync_TestCodeFix(string arguments, string oldAssertion, string newAssertion)
            => VerifyCSharpFix(arguments, oldAssertion, newAssertion);

        private void VerifyCSharpDiagnostic(string methodArguments, string assertion)
        {
            var source = GenerateCode.XunitAssertion(methodArguments, assertion);
            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithAllAnalyzers()
                .WithSources(source)
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, PackageReference.XunitAssert_2_5_1)
                .WithExpectedDiagnostics(new LegacyDiagnosticResult
                {
                    Id = AssertAnalyzer.XunitRule.Id,
                    Message = AssertAnalyzer.Message,
                    Locations = new DiagnosticResultLocation[]
                    {
                        new("Test0.cs", 15, 13)
                    },
                    Severity = DiagnosticSeverity.Info
                })
            );
        }

        private void VerifyCSharpFix(string methodArguments, string oldAssertion, string newAssertion)
        {
            var oldSource = GenerateCode.XunitAssertion(methodArguments, oldAssertion);
            var newSource = GenerateCode.XunitAssertion(methodArguments, newAssertion);

            DiagnosticVerifier.VerifyFix(new CodeFixVerifierArguments()
                .WithDiagnosticAnalyzer<AssertAnalyzer>()
                .WithCodeFixProvider<XunitCodeFixProvider>()
                .WithSources(oldSource)
                .WithFixedSources(newSource)
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, PackageReference.XunitAssert_2_5_1)
            );
        }
    }
}
