using System;
using FluentAssertions.Analyzers.TestUtils;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests.Tips;

[TestClass]
public class NunitTests
{
    [TestMethod]
    [Implemented]
    public void SupportExcludingMethods()
    {
        var source = GenerateCode.Nunit3Assertion("bool actual", "Assert.IsTrue(actual);");
        DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
            .WithAllAnalyzers()
            .WithSources(source)
            .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, PackageReference.Nunit_3_14_0)
            .WithAnalyzerConfigOption("ffa_excluded_methods", "M:NUnit.Framework.Assert.IsTrue(System.Boolean)")
            .WithExpectedDiagnostics()
        );
    }

    #region Assert.cs

    [DataTestMethod]
    [AssertionDiagnostic("Assert.Pass({0});")]
    [AssertionDiagnostic("Assert.Inconclusive({0});")]
    [AssertionDiagnostic("Assert.Ignore({0});")]
    [Implemented]
    public void Nunit3_NotReportedAsserts_TestAnalyzer(string assertion) => Nunit3VerifyNoDiagnostic(string.Empty, assertion);

    [DataTestMethod]
    [DataRow("Assert.Warn(\"warning message\");")]
    [DataRow("Assert.Warn(\"warning message {0} and more.\", DateTime.Now);")]
    [Implemented]
    public void Nunit3_SpecialNotReportedAsserts_TestAnalyzer(string assertion) => Nunit3VerifyNoDiagnostic(string.Empty, assertion);

    [DataTestMethod]
    [DataRow("Assert.Pass();")]
    [DataRow("Assert.Pass(\"passing message\");")]
    [DataRow("Assert.Inconclusive();")]
    [DataRow("Assert.Inconclusive(\"inconclusive message\");")]
    [DataRow("Assert.Ignore();")]
    [DataRow("Assert.Ignore(\"ignore message\");")]
    [DataRow("Assert.Warn(\"warning message\");")]
    [DataRow("Assert.Charlie();")]
    [Implemented]
    public void Nunit4_SpecialNotReportedAsserts_TestAnalyzer(string assertion) => Nunit4VerifyNoDiagnostic(string.Empty, assertion);

    #endregion

    #region Assert.Conditions.cs

    [DataTestMethod]
    [AssertionDiagnostic("Assert.True(actual{0});")]
    [AssertionDiagnostic("Assert.True(bool.Parse(\"true\"){0});")]
    [AssertionDiagnostic("Assert.IsTrue(actual{0});")]
    [AssertionDiagnostic("Assert.IsTrue(bool.Parse(\"true\"){0});")]
    [AssertionDiagnostic("Assert.That(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.True{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.False{0});")]
    [Implemented]
    public void Nunit3_AssertTrue_TestAnalyzer(string assertion) => Nunit3VerifyDiagnostic("bool actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.True(actual{0});")]
    [AssertionDiagnostic("ClassicAssert.True(bool.Parse(\"true\"){0});")]
    [AssertionDiagnostic("ClassicAssert.IsTrue(actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsTrue(bool.Parse(\"true\"){0});")]
    [AssertionDiagnostic("Assert.That(actual);")]
    [AssertionDiagnostic("Assert.That(actual, Is.True);")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.False);")]
    [Implemented]
    public void Nunit4_AssertTrue_TestAnalyzer(string assertion) => Nunit4VerifyDiagnostic("bool actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.True(actual{0});",
        newAssertion: "actual.Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.True(bool.Parse(\"true\"){0});",
        newAssertion: "bool.Parse(\"true\").Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.True(!actual{0});",
        newAssertion: "(!actual).Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.True(actual == false{0});",
        newAssertion: "(actual == false).Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsTrue(actual{0});",
        newAssertion: "actual.Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsTrue(bool.Parse(\"true\"){0});",
        newAssertion: "bool.Parse(\"true\").Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsTrue(!actual{0});",
        newAssertion: "(!actual).Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsTrue(actual == false{0});",
        newAssertion: "(actual == false).Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual{0});",
        newAssertion: "actual.Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.True{0});",
        newAssertion: "actual.Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.False{0});",
        newAssertion: "actual.Should().BeTrue({0});")]
    [Implemented]
    public void Nunit3_AssertTrue_TestCodeFix(string oldAssertion, string newAssertion) => Nunit3VerifyFix("bool actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.True(actual{0});",
        newAssertion: "actual.Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.True(bool.Parse(\"true\"){0});",
        newAssertion: "bool.Parse(\"true\").Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.True(!actual{0});",
        newAssertion: "(!actual).Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.True(actual == false{0});",
        newAssertion: "(actual == false).Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsTrue(actual{0});",
        newAssertion: "actual.Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsTrue(bool.Parse(\"true\"){0});",
        newAssertion: "bool.Parse(\"true\").Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsTrue(!actual{0});",
        newAssertion: "(!actual).Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsTrue(actual == false{0});",
        newAssertion: "(actual == false).Should().BeTrue({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual);",
        newAssertion: "actual.Should().BeTrue();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.True);",
        newAssertion: "actual.Should().BeTrue();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.False);",
        newAssertion: "actual.Should().BeTrue();")]
    [Implemented]
    public void Nunit4_AssertTrue_TestCodeFix(string oldAssertion, string newAssertion) => Nunit4VerifyFix("bool actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.False(actual{0});")]
    [AssertionDiagnostic("Assert.False(bool.Parse(\"false\"){0});")]
    [AssertionDiagnostic("Assert.IsFalse(actual{0});")]
    [AssertionDiagnostic("Assert.IsFalse(bool.Parse(\"false\"){0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.False{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.True{0});")]
    [Implemented]
    public void Nunit3_AssertFalse_TestAnalyzer(string assertion) => Nunit3VerifyDiagnostic("bool actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.False(actual{0});")]
    [AssertionDiagnostic("ClassicAssert.False(bool.Parse(\"false\"){0});")]
    [AssertionDiagnostic("ClassicAssert.IsFalse(actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsFalse(bool.Parse(\"false\"){0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.False);")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.True);")]
    [Implemented]
    public void Nunit4_AssertFalse_TestAnalyzer(string assertion) => Nunit4VerifyDiagnostic("bool actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.False(actual{0});",
        newAssertion: "actual.Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.False(bool.Parse(\"false\"){0});",
        newAssertion: "bool.Parse(\"false\").Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsFalse(actual{0});",
        newAssertion: "actual.Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsFalse(bool.Parse(\"false\"){0});",
        newAssertion: "bool.Parse(\"false\").Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.False{0});",
        newAssertion: "actual.Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.True{0});",
        newAssertion: "actual.Should().BeFalse({0});")]
    [Implemented]
    public void Nunit3_AssertFalse_TestCodeFix(string oldAssertion, string newAssertion) => Nunit3VerifyFix("bool actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.False(actual{0});",
        newAssertion: "actual.Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.False(bool.Parse(\"false\"){0});",
        newAssertion: "bool.Parse(\"false\").Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsFalse(actual{0});",
        newAssertion: "actual.Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsFalse(bool.Parse(\"false\"){0});",
        newAssertion: "bool.Parse(\"false\").Should().BeFalse({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.False);",
        newAssertion: "actual.Should().BeFalse();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.True);",
        newAssertion: "actual.Should().BeFalse();")]
    [Implemented]
    public void Nunit4_AssertFalse_TestCodeFix(string oldAssertion, string newAssertion) => Nunit4VerifyFix("bool actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.Null(actual{0});")]
    [AssertionDiagnostic("Assert.IsNull(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Null{0});")]
    [Implemented]
    public void Nunit3_AssertNull_TestAnalyzer(string assertion) => Nunit3VerifyDiagnostic("object actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.Null(actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsNull(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Null);")]
    [Implemented]
    public void Nunit4_AssertNull_TestAnalyzer(string assertion) => Nunit4VerifyDiagnostic("object actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.Null(actual{0});",
        newAssertion: "actual.Should().BeNull({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsNull(actual{0});",
        newAssertion: "actual.Should().BeNull({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Null{0});",
        newAssertion: "actual.Should().BeNull({0});")]
    [Implemented]
    public void Nunit3_AssertNull_TestCodeFix(string oldAssertion, string newAssertion) => Nunit3VerifyFix("object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.Null(actual{0});",
        newAssertion: "actual.Should().BeNull({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsNull(actual{0});",
        newAssertion: "actual.Should().BeNull({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Null);",
        newAssertion: "actual.Should().BeNull();")]
    [Implemented]
    public void Nunit4_AssertNull_TestCodeFix(string oldAssertion, string newAssertion) => Nunit4VerifyFix("object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.NotNull(actual{0});")]
    [AssertionDiagnostic("Assert.IsNotNull(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.Null{0});")]
    [Implemented]
    public void Nunit3_AssertNotNull_TestAnalyzer(string assertion) => Nunit3VerifyDiagnostic("object actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.NotNull(actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsNotNull(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.Null);")]
    [Implemented]
    public void Nunit4_AssertNotNull_TestAnalyzer(string assertion) => Nunit4VerifyDiagnostic("object actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.NotNull(actual{0});",
        newAssertion: "actual.Should().NotBeNull({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsNotNull(actual{0});",
        newAssertion: "actual.Should().NotBeNull({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.Null{0});",
        newAssertion: "actual.Should().NotBeNull({0});")]
    [Implemented]
    public void Nunit3_AssertNotNull_TestCodeFix(string oldAssertion, string newAssertion) => Nunit3VerifyFix("object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.NotNull(actual{0});",
        newAssertion: "actual.Should().NotBeNull({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsNotNull(actual{0});",
        newAssertion: "actual.Should().NotBeNull({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.Null);",
        newAssertion: "actual.Should().NotBeNull();")]
    [Implemented]
    public void Nunit4_AssertNotNull_TestCodeFix(string oldAssertion, string newAssertion) => Nunit4VerifyFix("object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.IsNaN(actual{0});")]
    [Implemented]
    public void Nunit3_AssertIsNaN_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("double actual", assertion);
        Nunit3VerifyNoFix("double actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.IsNaN(actual{0});")]
    [Implemented]
    public void Nunit4_AssertIsNaN_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("double actual", assertion);
        Nunit4VerifyNoFix("double actual", assertion);
    }

    // IsEmpty
    [DataTestMethod]
    [AssertionDiagnostic("Assert.IsEmpty(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Empty{0});")]
    [AssertionDiagnostic("CollectionAssert.IsEmpty(actual{0});")]
    [Implemented]
    public void Nunit3_AssertIsEmpty_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("object[] actual", assertion);
        Nunit3VerifyDiagnostic("IEnumerable<int> actual", assertion);
        Nunit3VerifyDiagnostic("IEnumerable actual", assertion);
        Nunit3VerifyDiagnostic("string actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.IsEmpty(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Empty);")]
    [AssertionDiagnostic("CollectionAssert.IsEmpty(actual{0});")]
    [Implemented]
    public void Nunit4_AssertIsEmpty_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("object[] actual", assertion);
        Nunit4VerifyDiagnostic("IEnumerable<int> actual", assertion);
        Nunit4VerifyDiagnostic("IEnumerable actual", assertion);
        Nunit4VerifyDiagnostic("string actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsEmpty(actual{0});",
        newAssertion: "actual.Should().BeEmpty({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Empty{0});",
        newAssertion: "actual.Should().BeEmpty({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.Count.EqualTo(0){0});",
        newAssertion: "actual.Should().BeEmpty({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.Count.Zero{0});",
        newAssertion: "actual.Should().BeEmpty({0});")]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.IsEmpty(actual{0});",
        newAssertion: "actual.Should().BeEmpty({0});")]
    [Implemented]
    public void Nunit3_AssertIsEmpty_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyFix("object[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("IEnumerable<int> actual", oldAssertion, newAssertion);
        Nunit3VerifyNoFix("IEnumerable actual", oldAssertion);
        Nunit3VerifyFix("string actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsEmpty(actual{0});",
        newAssertion: "actual.Should().BeEmpty({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Empty);",
        newAssertion: "actual.Should().BeEmpty();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.Count.EqualTo(0));",
        newAssertion: "actual.Should().BeEmpty();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.Count.Zero);",
        newAssertion: "actual.Should().BeEmpty();")]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.IsEmpty(actual{0});",
        newAssertion: "actual.Should().BeEmpty({0});")]
    [Implemented]
    public void Nunit4_AssertIsEmpty_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyFix("object[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("IEnumerable<int> actual", oldAssertion, newAssertion);
        Nunit4VerifyNoFix("IEnumerable actual", oldAssertion);
        Nunit4VerifyFix("string actual", oldAssertion, newAssertion);
    }

    // IsNotEmpty
    [DataTestMethod]
    [AssertionDiagnostic("Assert.IsNotEmpty(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.Empty{0});")]
    [AssertionDiagnostic("CollectionAssert.IsNotEmpty(actual{0});")]
    [Implemented]
    public void Nunit3_AssertIsNotEmpty_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("object[] actual", assertion);
        Nunit3VerifyDiagnostic("IEnumerable<int> actual", assertion);
        Nunit3VerifyDiagnostic("IEnumerable actual", assertion);
        Nunit3VerifyDiagnostic("string actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.IsNotEmpty(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.Empty);")]
    [AssertionDiagnostic("CollectionAssert.IsNotEmpty(actual{0});")]
    [Implemented]
    public void Nunit4_AssertIsNotEmpty_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("object[] actual", assertion);
        Nunit4VerifyDiagnostic("IEnumerable<int> actual", assertion);
        Nunit4VerifyDiagnostic("IEnumerable actual", assertion);
        Nunit4VerifyDiagnostic("string actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsNotEmpty(actual{0});",
        newAssertion: "actual.Should().NotBeEmpty({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.Empty{0});",
        newAssertion: "actual.Should().NotBeEmpty({0});")]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.IsNotEmpty(actual{0});",
        newAssertion: "actual.Should().NotBeEmpty({0});")]
    [Implemented]
    public void Nunit3_AssertIsNotEmpty_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyFix("object[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("IEnumerable<int> actual", oldAssertion, newAssertion);
        Nunit3VerifyNoFix("IEnumerable actual", oldAssertion);
        Nunit3VerifyFix("string actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsNotEmpty(actual{0});",
        newAssertion: "actual.Should().NotBeEmpty({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.Empty);",
        newAssertion: "actual.Should().NotBeEmpty();")]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.IsNotEmpty(actual{0});",
        newAssertion: "actual.Should().NotBeEmpty({0});")]
    [Implemented]
    public void Nunit4_AssertIsNotEmpty_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyFix("object[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("IEnumerable<int> actual", oldAssertion, newAssertion);
        Nunit4VerifyNoFix("IEnumerable actual", oldAssertion);
        Nunit4VerifyFix("string actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("Assert.Zero(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Zero{0});")]
    [Implemented]
    public void Nunit3_AssertZero_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("int actual", assertion);
        Nunit3VerifyDiagnostic("uint actual", assertion);
        Nunit3VerifyDiagnostic("long actual", assertion);
        Nunit3VerifyDiagnostic("ulong actual", assertion);
        Nunit3VerifyDiagnostic("float actual", assertion);
        Nunit3VerifyDiagnostic("double actual", assertion);
        Nunit3VerifyDiagnostic("decimal actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.Zero(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Zero);")]
    [Implemented]
    public void Nunit4_AssertZero_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("int actual", assertion);
        Nunit4VerifyDiagnostic("uint actual", assertion);
        Nunit4VerifyDiagnostic("long actual", assertion);
        Nunit4VerifyDiagnostic("ulong actual", assertion);
        Nunit4VerifyDiagnostic("float actual", assertion);
        Nunit4VerifyDiagnostic("double actual", assertion);
        Nunit4VerifyDiagnostic("decimal actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.Zero(actual{0});",
        newAssertion: "actual.Should().Be(0{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Zero{0});",
        newAssertion: "actual.Should().Be(0{0});")]
    [Implemented]
    public void Nunit3_AssertZero_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyFix("int actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("uint actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("long actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("ulong actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("float actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("double actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("decimal actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.Zero(actual{0});",
        newAssertion: "actual.Should().Be(0{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Zero);",
        newAssertion: "actual.Should().Be(0);")]
    [Implemented]
    public void Nunit4_AssertZero_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyFix("int actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("uint actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("long actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("ulong actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("float actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("double actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("decimal actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("Assert.NotZero(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.Zero{0});")]
    [Implemented]
    public void Nunit3_AssertNotZero_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("int actual", assertion);
        Nunit3VerifyDiagnostic("uint actual", assertion);
        Nunit3VerifyDiagnostic("long actual", assertion);
        Nunit3VerifyDiagnostic("ulong actual", assertion);
        Nunit3VerifyDiagnostic("float actual", assertion);
        Nunit3VerifyDiagnostic("double actual", assertion);
        Nunit3VerifyDiagnostic("decimal actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.NotZero(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.Zero);")]
    [Implemented]
    public void Nunit4_AssertNotZero_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("int actual", assertion);
        Nunit4VerifyDiagnostic("uint actual", assertion);
        Nunit4VerifyDiagnostic("long actual", assertion);
        Nunit4VerifyDiagnostic("ulong actual", assertion);
        Nunit4VerifyDiagnostic("float actual", assertion);
        Nunit4VerifyDiagnostic("double actual", assertion);
        Nunit4VerifyDiagnostic("decimal actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.NotZero(actual{0});",
        newAssertion: "actual.Should().NotBe(0{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.Zero{0});",
        newAssertion: "actual.Should().NotBe(0{0});")]
    [Implemented]
    public void Nunit3_AssertNotZero_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyFix("int actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("uint actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("long actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("ulong actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("float actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("double actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("decimal actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.NotZero(actual{0});",
        newAssertion: "actual.Should().NotBe(0{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.Zero);",
        newAssertion: "actual.Should().NotBe(0);")]
    [Implemented]
    public void Nunit4_AssertNotZero_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyFix("int actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("uint actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("long actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("ulong actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("float actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("double actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("decimal actual", oldAssertion, newAssertion);
    }

    // Positive
    [DataTestMethod]
    [AssertionDiagnostic("Assert.Positive(actual{0});")]
    [Implemented]
    public void Nunit3_AssertPositive_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("int actual", assertion);
        Nunit3VerifyDiagnostic("uint actual", assertion);
        Nunit3VerifyDiagnostic("long actual", assertion);
        Nunit3VerifyDiagnostic("ulong actual", assertion);
        Nunit3VerifyDiagnostic("float actual", assertion);
        Nunit3VerifyDiagnostic("double actual", assertion);
        Nunit3VerifyDiagnostic("decimal actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.Positive(actual{0});")]
    [Implemented]
    public void Nunit4_AssertPositive_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("int actual", assertion);
        Nunit4VerifyDiagnostic("uint actual", assertion);
        Nunit4VerifyDiagnostic("long actual", assertion);
        Nunit4VerifyDiagnostic("ulong actual", assertion);
        Nunit4VerifyDiagnostic("float actual", assertion);
        Nunit4VerifyDiagnostic("double actual", assertion);
        Nunit4VerifyDiagnostic("decimal actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.Positive(actual{0});",
        newAssertion: "actual.Should().BePositive({0});")]
    [Implemented]
    public void Nunit3_AssertPositive_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyFix("int actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("uint actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("long actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("ulong actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("float actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("double actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("decimal actual", oldAssertion, newAssertion);
    }

    // Negative
    [DataTestMethod]
    [AssertionDiagnostic("Assert.Negative(actual{0});")]
    [Implemented]
    public void Nunit3_AssertNegative_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("int actual", assertion);
        Nunit3VerifyDiagnostic("uint actual", assertion);
        Nunit3VerifyDiagnostic("long actual", assertion);
        Nunit3VerifyDiagnostic("ulong actual", assertion);
        Nunit3VerifyDiagnostic("float actual", assertion);
        Nunit3VerifyDiagnostic("double actual", assertion);
        Nunit3VerifyDiagnostic("decimal actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.Negative(actual{0});")]
    [Implemented]
    public void Nunit4_AssertNegative_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("int actual", assertion);
        Nunit4VerifyDiagnostic("uint actual", assertion);
        Nunit4VerifyDiagnostic("long actual", assertion);
        Nunit4VerifyDiagnostic("ulong actual", assertion);
        Nunit4VerifyDiagnostic("float actual", assertion);
        Nunit4VerifyDiagnostic("double actual", assertion);
        Nunit4VerifyDiagnostic("decimal actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.Negative(actual{0});",
        newAssertion: "actual.Should().BeNegative({0});")]
    [Implemented]
    public void Nunit3_AssertNegative_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyFix("int actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("uint actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("long actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("ulong actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("float actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("double actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("decimal actual", oldAssertion, newAssertion);
    }

    #endregion

    #region Assert.Comparisons.cs

    [DataTestMethod]
    [AssertionDiagnostic("Assert.Greater(arg1, arg2{0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.GreaterThan(arg2){0});")]
    [Implemented]
    public void Nunit3_AssertGreater_TestAnalyzer(string assertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit3VerifyDiagnostic(comparableArgument, assertion);
        }
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.Greater(arg1, arg2{0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.GreaterThan(arg2));")]
    [Implemented]
    public void Nunit4_AssertGreater_TestAnalyzer(string assertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit4VerifyDiagnostic(comparableArgument, assertion);
        }
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.Greater(arg1, arg2{0});",
        newAssertion: "arg1.Should().BeGreaterThan(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.GreaterThan(arg2){0});",
        newAssertion: "arg1.Should().BeGreaterThan(arg2{0});")]
    [Implemented]
    public void Nunit3_AssertGreater_TestCodeFix(string oldAssertion, string newAssertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit3VerifyFix(comparableArgument, oldAssertion, newAssertion);
        }
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.Greater(arg1, arg2{0});",
        newAssertion: "arg1.Should().BeGreaterThan(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.GreaterThan(arg2));",
        newAssertion: "arg1.Should().BeGreaterThan(arg2);")]
    [Implemented]
    public void Nunit4_AssertGreater_TestCodeFix(string oldAssertion, string newAssertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit4VerifyFix(comparableArgument, oldAssertion, newAssertion);
        }
    }

    [DataTestMethod]
    [AssertionDiagnostic("Assert.GreaterOrEqual(arg1, arg2{0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2){0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.AtLeast(arg2){0});")]
    [Implemented]
    public void Nunit3_AssertGreaterOrEqual_TestAnalyzer(string assertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit3VerifyDiagnostic(comparableArgument, assertion);
        }
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.GreaterOrEqual(arg1, arg2{0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2));")]
    [AssertionDiagnostic("Assert.That(arg1, Is.AtLeast(arg2));")]
    [Implemented]
    public void Nunit4_AssertGreaterOrEqual_TestAnalyzer(string assertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit4VerifyDiagnostic(comparableArgument, assertion);
        }
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.GreaterOrEqual(arg1, arg2{0});",
        newAssertion: "arg1.Should().BeGreaterOrEqualTo(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2){0});",
        newAssertion: "arg1.Should().BeGreaterOrEqualTo(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.AtLeast(arg2){0});",
        newAssertion: "arg1.Should().BeGreaterOrEqualTo(arg2{0});")]
    [Implemented]
    public void Nunit3_AssertGreaterOrEqual_TestCodeFix(string oldAssertion, string newAssertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit3VerifyFix(comparableArgument, oldAssertion, newAssertion);
        }
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.GreaterOrEqual(arg1, arg2{0});",
        newAssertion: "arg1.Should().BeGreaterOrEqualTo(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2));",
        newAssertion: "arg1.Should().BeGreaterOrEqualTo(arg2);")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.AtLeast(arg2));",
        newAssertion: "arg1.Should().BeGreaterOrEqualTo(arg2);")]
    [Implemented]
    public void Nunit4_AssertGreaterOrEqual_TestCodeFix(string oldAssertion, string newAssertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit4VerifyFix(comparableArgument, oldAssertion, newAssertion);
        }
    }

    [DataTestMethod]
    [AssertionDiagnostic("Assert.Less(arg1, arg2{0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.LessThan(arg2){0});")]
    [Implemented]
    public void Nunit3_AssertLess_TestAnalyzer(string assertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit3VerifyDiagnostic(comparableArgument, assertion);
        }
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.Less(arg1, arg2{0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.LessThan(arg2));")]
    [Implemented]
    public void Nunit4_AssertLess_TestAnalyzer(string assertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit4VerifyDiagnostic(comparableArgument, assertion);
        }
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.Less(arg1, arg2{0});",
        newAssertion: "arg1.Should().BeLessThan(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.LessThan(arg2){0});",
        newAssertion: "arg1.Should().BeLessThan(arg2{0});")]
    [Implemented]
    public void Nunit3_AssertLess_TestCodeFix(string oldAssertion, string newAssertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit3VerifyFix(comparableArgument, oldAssertion, newAssertion);
        }
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.Less(arg1, arg2{0});",
        newAssertion: "arg1.Should().BeLessThan(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.LessThan(arg2));",
        newAssertion: "arg1.Should().BeLessThan(arg2);")]
    [Implemented]
    public void Nunit4_AssertLess_TestCodeFix(string oldAssertion, string newAssertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit4VerifyFix(comparableArgument, oldAssertion, newAssertion);
        }
    }

    [DataTestMethod]
    [AssertionDiagnostic("Assert.LessOrEqual(arg1, arg2{0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.LessThanOrEqualTo(arg2){0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.AtMost(arg2){0});")]
    [Implemented]
    public void Nunit3_AssertLessOrEqual_TestAnalyzer(string assertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit3VerifyDiagnostic(comparableArgument, assertion);
        }
    }

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.LessOrEqual(arg1, arg2{0});")]
    [AssertionDiagnostic("Assert.That(arg1, Is.LessThanOrEqualTo(arg2));")]
    [AssertionDiagnostic("Assert.That(arg1, Is.AtMost(arg2));")]
    [Implemented]
    public void Nunit4_AssertLessOrEqual_TestAnalyzer(string assertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit4VerifyDiagnostic(comparableArgument, assertion);
        }
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.LessOrEqual(arg1, arg2{0});",
        newAssertion: "arg1.Should().BeLessOrEqualTo(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.LessThanOrEqualTo(arg2){0});",
        newAssertion: "arg1.Should().BeLessOrEqualTo(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.AtMost(arg2){0});",
        newAssertion: "arg1.Should().BeLessOrEqualTo(arg2{0});")]
    [Implemented]
    public void Nunit3_AssertLessOrEqual_TestCodeFix(string oldAssertion, string newAssertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit3VerifyFix(comparableArgument, oldAssertion, newAssertion);
        }
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.LessOrEqual(arg1, arg2{0});",
        newAssertion: "arg1.Should().BeLessOrEqualTo(arg2{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.LessThanOrEqualTo(arg2));",
        newAssertion: "arg1.Should().BeLessOrEqualTo(arg2);")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(arg1, Is.AtMost(arg2));",
        newAssertion: "arg1.Should().BeLessOrEqualTo(arg2);")]
    [Implemented]
    public void Nunit4_AssertLessOrEqual_TestCodeFix(string oldAssertion, string newAssertion)
    {
        foreach (var comparableArgument in ComparableArguments)
        {
            Nunit4VerifyFix(comparableArgument, oldAssertion, newAssertion);
        }
    }

    private static readonly string[] ComparableArguments = Array.ConvertAll(new string[] { "int", "uint", "long", "ulong", "float", "double", "decimal" }, x => $"{x} arg1, {x} arg2");

    #endregion

    #region Assert.Equality.cs

    [DataTestMethod]
    [AssertionDiagnostic("Assert.AreEqual(expected, actual, delta{0});")]
    [AssertionDiagnostic("Assert.AreEqual(actual, 4.2d, delta{0});")]
    [Implemented]
    public void Nunit3_AssertEqualDouble_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("double expected, double actual, double delta", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.AreEqual(expected, actual, delta{0});")]
    [AssertionDiagnostic("ClassicAssert.AreEqual(actual, 4.2d, delta{0});")]
    [Implemented]
    public void Nunit4_AssertEqualDouble_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("double expected, double actual, double delta", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.AreEqual(expected, actual, delta{0});",
        newAssertion: "actual.Should().BeApproximately(expected, delta{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.AreEqual(actual, 4.2d, delta{0});",
        newAssertion: "actual.Should().BeApproximately(4.2d, delta{0});")]
    [Implemented]
    public void Nunit3_AssertEqualDouble_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("double expected, double actual, double delta", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.AreEqual(expected, actual, delta{0});",
        newAssertion: "actual.Should().BeApproximately(expected, delta{0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.AreEqual(actual, 4.2d, delta{0});",
        newAssertion: "actual.Should().BeApproximately(4.2d, delta{0});")]
    [Implemented]
    public void Nunit4_AssertEqualDouble_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("double expected, double actual, double delta", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.AreEqual(expected, actual{0});")]
    [AssertionDiagnostic("Assert.AreEqual(actual, \"foo\"{0});")]
    [Implemented]
    public void Nunit3_AssertEqualObject_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object expected, object actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.AreEqual(expected, actual{0});")]
    [AssertionDiagnostic("ClassicAssert.AreEqual(actual, \"foo\"{0});")]
    [Implemented]
    public void Nunit4_AssertEqualObject_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object expected, object actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.AreEqual(expected, actual{0});",
        newAssertion: "actual.Should().Be(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.AreEqual(actual, \"foo\"{0});",
        newAssertion: "actual.Should().Be(\"foo\"{0});")]
    [Implemented]
    public void Nunit3_AssertEqualObject_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("object expected, object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.AreEqual(expected, actual{0});",
        newAssertion: "actual.Should().Be(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.AreEqual(actual, \"foo\"{0});",
        newAssertion: "actual.Should().Be(\"foo\"{0});")]
    [Implemented]
    public void Nunit4_AssertEqualObject_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("object expected, object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.AreNotEqual(expected, actual{0});")]
    [AssertionDiagnostic("Assert.AreNotEqual(actual, \"foo\"{0});")]
    [Implemented]
    public void Nunit3_AssertNotEqualObject_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object expected, object actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.AreNotEqual(expected, actual{0});")]
    [AssertionDiagnostic("ClassicAssert.AreNotEqual(actual, \"foo\"{0});")]
    [Implemented]
    public void Nunit4_AssertNotEqualObject_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object expected, object actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.AreNotEqual(expected, actual{0});",
        newAssertion: "actual.Should().NotBe(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.AreNotEqual(actual, \"foo\"{0});",
        newAssertion: "actual.Should().NotBe(\"foo\"{0});")]
    [Implemented]
    public void Nunit3_AssertNotEqualObject_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("object expected, object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.AreNotEqual(expected, actual{0});",
        newAssertion: "actual.Should().NotBe(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.AreNotEqual(actual, \"foo\"{0});",
        newAssertion: "actual.Should().NotBe(\"foo\"{0});")]
    [Implemented]
    public void Nunit4_AssertNotEqualObject_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("object expected, object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.AreSame(expected, actual{0});")]
    [Implemented]
    public void Nunit3_AssertAreSame_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object expected, object actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.AreSame(expected, actual{0});")]
    [Implemented]
    public void Nunit4_AssertAreSame_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object expected, object actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.AreSame(expected, actual{0});",
        newAssertion: "actual.Should().BeSameAs(expected{0});")]
    [Implemented]
    public void Nunit3_AssertAreSame_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("object expected, object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.AreSame(expected, actual{0});",
        newAssertion: "actual.Should().BeSameAs(expected{0});")]
    [Implemented]
    public void Nunit4_AssertAreSame_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("object expected, object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.AreNotSame(expected, actual{0});")]
    [Implemented]
    public void Nunit3_AssertAreNotSame_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object expected, object actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.AreNotSame(expected, actual{0});")]
    [Implemented]
    public void Nunit4_AssertAreNotSame_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object expected, object actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.AreNotSame(expected, actual{0});",
        newAssertion: "actual.Should().NotBeSameAs(expected{0});")]
    [Implemented]
    public void Nunit3_AssertAreNotSame_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("object expected, object actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.AreNotSame(expected, actual{0});",
        newAssertion: "actual.Should().NotBeSameAs(expected{0});")]
    [Implemented]
    public void Nunit4_AssertAreNotSame_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("object expected, object actual", oldAssertion, newAssertion);

    #endregion

    #region Assert.Types.cs

    [DataTestMethod]
    [AssertionDiagnostic("Assert.IsAssignableFrom(typeof(string), actual{0});")]
    [AssertionDiagnostic("Assert.IsAssignableFrom(expected, actual{0});")]
    [AssertionDiagnostic("Assert.IsAssignableFrom<string>(actual{0});")]
    [Implemented]
    public void Nunit3_AssertIsAssignableFrom_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object actual, Type expected", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.IsAssignableFrom(typeof(string), actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsAssignableFrom(expected, actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsAssignableFrom<string>(actual{0});")]
    [Implemented]
    public void Nunit4_AssertIsAssignableFrom_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object actual, Type expected", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsAssignableFrom(typeof(string), actual{0});",
        newAssertion: "actual.Should().BeAssignableTo<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsAssignableFrom(expected, actual{0});",
        newAssertion: "actual.Should().BeAssignableTo(expected{0});")]
    [Implemented]
    public void Nunit3_AssertIsAssignableFrom_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("object actual, Type expected", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsAssignableFrom(typeof(string), actual{0});",
        newAssertion: "actual.Should().BeAssignableTo<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsAssignableFrom(expected, actual{0});",
        newAssertion: "actual.Should().BeAssignableTo(expected{0});")]
    [Implemented]
    public void Nunit4_AssertIsAssignableFrom_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("object actual, Type expected", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.IsNotAssignableFrom(typeof(string), actual{0});")]
    [AssertionDiagnostic("Assert.IsNotAssignableFrom(expected, actual{0});")]
    [AssertionDiagnostic("Assert.IsNotAssignableFrom<string>(actual{0});")]
    [Implemented]
    public void Nunit3_AssertIsNotAssignableFrom_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object actual, Type expected", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.IsNotAssignableFrom(typeof(string), actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsNotAssignableFrom(expected, actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsNotAssignableFrom<string>(actual{0});")]
    [Implemented]
    public void Nunit4_AssertIsNotAssignableFrom_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object actual, Type expected", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsNotAssignableFrom(typeof(string), actual{0});",
        newAssertion: "actual.Should().NotBeAssignableTo<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsNotAssignableFrom(expected, actual{0});",
        newAssertion: "actual.Should().NotBeAssignableTo(expected{0});")]
    [Implemented]
    public void Nunit3_AssertIsNotAssignableFrom_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("object actual, Type expected", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsNotAssignableFrom(typeof(string), actual{0});",
        newAssertion: "actual.Should().NotBeAssignableTo<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsNotAssignableFrom(expected, actual{0});",
        newAssertion: "actual.Should().NotBeAssignableTo(expected{0});")]
    [Implemented]
    public void Nunit4_AssertIsNotAssignableFrom_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("object actual, Type expected", oldAssertion, newAssertion);

    // void IsInstanceOf(Type expected, object? actual, string message, params object?[]? args)
    [DataTestMethod]
    [AssertionDiagnostic("Assert.IsInstanceOf(typeof(string), actual{0});")]
    [AssertionDiagnostic("Assert.IsInstanceOf(expected, actual{0});")]
    [AssertionDiagnostic("Assert.IsInstanceOf<string>(actual{0});")]
    [Implemented]
    public void Nunit3_AssertIsInstanceOf_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object actual, Type expected", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.IsInstanceOf(typeof(string), actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsInstanceOf(expected, actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsInstanceOf<string>(actual{0});")]
    [Implemented]
    public void Nunit4_AssertIsInstanceOf_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object actual, Type expected", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsInstanceOf(typeof(string), actual{0});",
        newAssertion: "actual.Should().BeOfType<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsInstanceOf(expected, actual{0});",
        newAssertion: "actual.Should().BeOfType(expected{0});")]
    [Implemented]
    public void Nunit3_AssertIsInstanceOf_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("object actual, Type expected", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsInstanceOf(typeof(string), actual{0});",
        newAssertion: "actual.Should().BeOfType<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsInstanceOf(expected, actual{0});",
        newAssertion: "actual.Should().BeOfType(expected{0});")]
    [Implemented]
    public void Nunit4_AssertIsInstanceOf_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("object actual, Type expected", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.IsNotInstanceOf(typeof(string), actual{0});")]
    [AssertionDiagnostic("Assert.IsNotInstanceOf(expected, actual{0});")]
    [AssertionDiagnostic("Assert.IsNotInstanceOf<string>(actual{0});")]
    [Implemented]
    public void Nunit3_AssertIsNotInstanceOf_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object actual, Type expected", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.IsNotInstanceOf(typeof(string), actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsNotInstanceOf(expected, actual{0});")]
    [AssertionDiagnostic("ClassicAssert.IsNotInstanceOf<string>(actual{0});")]
    [Implemented]
    public void Nunit4_AssertIsNotInstanceOf_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object actual, Type expected", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsNotInstanceOf(typeof(string), actual{0});",
        newAssertion: "actual.Should().NotBeOfType<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.IsNotInstanceOf(expected, actual{0});",
        newAssertion: "actual.Should().NotBeOfType(expected{0});")]
    [Implemented]
    public void Nunit3_AssertIsNotInstanceOf_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit3VerifyFix("object actual, Type expected", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsNotInstanceOf(typeof(string), actual{0});",
        newAssertion: "actual.Should().NotBeOfType<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.IsNotInstanceOf(expected, actual{0});",
        newAssertion: "actual.Should().NotBeOfType(expected{0});")]
    [Implemented]
    public void Nunit4_AssertIsNotInstanceOf_TestCodeFix(string oldAssertion, string newAssertion)
        => Nunit4VerifyFix("object actual, Type expected", oldAssertion, newAssertion);

    #endregion

    #region Assert.Contains.cs

    [DataTestMethod]
    [AssertionDiagnostic("Assert.Contains(expected, actual{0});")]
    [Implemented]
    public void Nunit3_AssertContains_ICollection_TestCodeNoFix(string assertion)
        => Nunit3VerifyNoFix("object expected, ICollection actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.Contains(expected, actual{0});")]
    [Implemented]
    public void Nunit4_AssertContains_ICollection_TestCodeNoFix(string assertion)
        => Nunit4VerifyNoFix("object expected, ICollection actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("Assert.Contains(expected, actual{0});")]
    [Implemented]
    public void Nunit3_AssertContains_TestAnalyzer(string assertion)
        => Nunit3VerifyDiagnostic("object expected, string[] actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("ClassicAssert.Contains(expected, actual{0});")]
    [Implemented]
    public void Nunit4_AssertContains_TestAnalyzer(string assertion)
        => Nunit4VerifyDiagnostic("object expected, string[] actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "Assert.Contains(expected, actual{0});",
        newAssertion: "actual.Should().Contain(expected{0});")]
    [Implemented]
    public void Nunit3_AssertContains_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyFix("string expected, string[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("string expected, List<string> actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("string expected, object[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("string expected, List<object> actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("DateTime expected, DateTime[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("DateTime expected, List<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "ClassicAssert.Contains(expected, actual{0});",
        newAssertion: "actual.Should().Contain(expected{0});")]
    [Implemented]
    public void Nunit4_AssertContains_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyFix("string expected, string[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("string expected, List<string> actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("string expected, object[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("string expected, List<object> actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("DateTime expected, DateTime[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("DateTime expected, List<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, string[] actual",
        oldAssertion: "Assert.Contains(expected, actual);",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.Contains(expected, actual);",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, DateTime[] actual",
        oldAssertion: "Assert.Contains(expected, actual);",
        newAssertion: "actual.Should().Contain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<DateTime> actual",
        oldAssertion: "Assert.Contains(expected, actual);",
        newAssertion: "actual.Should().Contain((DateTime)expected);")]
    [Implemented]
    public void Nunit3_AssertContains_WithCasting_TestCodeFix(string methodArguments, string oldAssertion, string newAssertion)
        => Nunit3VerifyFix(methodArguments, oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, string[] actual",
        oldAssertion: "ClassicAssert.Contains(expected, actual);",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "ClassicAssert.Contains(expected, actual);",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, DateTime[] actual",
        oldAssertion: "ClassicAssert.Contains(expected, actual);",
        newAssertion: "actual.Should().Contain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<DateTime> actual",
        oldAssertion: "ClassicAssert.Contains(expected, actual);",
        newAssertion: "actual.Should().Contain((DateTime)expected);")]
    [Implemented]
    public void Nunit4_AssertContains_WithCasting_TestCodeFix(string methodArguments, string oldAssertion, string newAssertion)
        => Nunit4VerifyFix(methodArguments, oldAssertion, newAssertion);

    #endregion

    #region CollectionAssert.cs

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AreEqual(expected, actual{0});")]
    [AssertionDiagnostic("CollectionAssert.AreEqual(expected, actual, comparer{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.EqualTo(expected){0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAreEqual_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("IEnumerable expected, IEnumerable actual, IComparer comparer", assertion);
        Nunit3VerifyDiagnostic("ICollection<string> expected, ICollection<string> actual, Comparer<string> comparer", assertion);
        Nunit3VerifyDiagnostic("ICollection<DateTime> expected, ICollection<DateTime> actual, Comparer<DateTime> comparer", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AreEqual(expected, actual{0});")]
    [AssertionDiagnostic("CollectionAssert.AreEqual(expected, actual, comparer{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.EqualTo(expected));")]
    [Implemented]
    public void Nunit4_CollectionAssertAreEqual_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("IEnumerable expected, IEnumerable actual, IComparer comparer", assertion);
        Nunit4VerifyDiagnostic("ICollection<string> expected, ICollection<string> actual, Comparer<string> comparer", assertion);
        Nunit4VerifyDiagnostic("ICollection<DateTime> expected, ICollection<DateTime> actual, Comparer<DateTime> comparer", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AreEqual(expected, actual{0});",
        newAssertion: "actual.Should().Equal(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.EqualTo(expected){0});",
        newAssertion: "actual.Should().Equal(expected{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAreEqual_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyFix("ICollection<string> expected, ICollection<string> actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("ICollection<DateTime> expected, ICollection<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AreEqual(expected, actual{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAreEqual_NoFix_NonGenericIEnumerable_TestCodeFix(string assertion)
    {
        Nunit3VerifyNoFix("IEnumerable expected, IEnumerable actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AreEqual(expected, actual, comparer{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAreEqual_NoFix_IComparer_TestCodeFix(string assertion)
    {
        Nunit3VerifyNoFix("ICollection<string> expected, ICollection<string> actual, IComparer comparer", assertion);
        Nunit3VerifyNoFix("IEnumerable<DateTime> expected, IEnumerable<DateTime> actual, IComparer comparer", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AreEqual(expected, actual{0});",
        newAssertion: "actual.Should().Equal(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.EqualTo(expected));",
        newAssertion: "actual.Should().Equal(expected);")]
    [Implemented]
    public void Nunit4_CollectionAssertAreEqual_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyNoFix("IEnumerable expected, IEnumerable actual", oldAssertion);
        Nunit4VerifyFix("ICollection<string> expected, ICollection<string> actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("ICollection<DateTime> expected, ICollection<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AreEqual(expected, actual{0});")]
    [Implemented]
    public void Nunit4_CollectionAssertAreEqual_NoFix_NonGenericIEnumerable_TestCodeFix(string assertion)
    {
        Nunit4VerifyNoFix("IEnumerable expected, IEnumerable actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AreEqual(expected, actual, comparer{0});")]
    [Implemented]
    public void Nunit4_CollectionAssertAreEqual_NoFix_IComparer_TestCodeFix(string assertion)
    {
        Nunit4VerifyNoFix("ICollection<string> expected, ICollection<string> actual, IComparer comparer", assertion);
        Nunit4VerifyNoFix("IEnumerable<DateTime> expected, IEnumerable<DateTime> actual, IComparer comparer", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AreNotEqual(expected, actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.EqualTo(expected){0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAreNotEqual_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("IEnumerable expected, IEnumerable actual", assertion);
        Nunit3VerifyDiagnostic("ICollection<string> expected, ICollection<string> actual", assertion);
        Nunit3VerifyDiagnostic("ICollection<DateTime> expected, ICollection<DateTime> actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AreNotEqual(expected, actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Not.EqualTo(expected));")]
    [Implemented]
    public void Nunit4_CollectionAssertAreNotEqual_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("IEnumerable expected, IEnumerable actual", assertion);
        Nunit4VerifyDiagnostic("ICollection<string> expected, ICollection<string> actual", assertion);
        Nunit4VerifyDiagnostic("ICollection<DateTime> expected, ICollection<DateTime> actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AreNotEqual(expected, actual{0});",
        newAssertion: "actual.Should().NotEqual(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.EqualTo(expected){0});",
        newAssertion: "actual.Should().NotEqual(expected{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAreNotEqual_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyNoFix("IEnumerable expected, IEnumerable actual", oldAssertion);
        Nunit3VerifyFix("ICollection<string> expected, ICollection<string> actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("ICollection<DateTime> expected, ICollection<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AreNotEqual(expected, actual{0});",
        newAssertion: "actual.Should().NotEqual(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Not.EqualTo(expected));",
        newAssertion: "actual.Should().NotEqual(expected);")]
    [Implemented]
    public void Nunit4_CollectionAssertAreNotEqual_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyNoFix("IEnumerable expected, IEnumerable actual", oldAssertion);
        Nunit4VerifyFix("ICollection<string> expected, ICollection<string> actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("ICollection<DateTime> expected, ICollection<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.Contains(actual, expected{0});")]
    [AssertionDiagnostic("Assert.That(actual, Has.Member(expected){0});")]
    [AssertionDiagnostic("Assert.That(actual, Does.Contain(expected){0});")]
    [AssertionDiagnostic("Assert.That(actual, Contains.Item(expected){0});")]
    [Implemented]
    public void Nunit3_CollectionAssertContains_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("object expected, IEnumerable actual", assertion);
        Nunit3VerifyDiagnostic("object expected, string[] actual", assertion);
        Nunit3VerifyDiagnostic("object expected, List<string> actual", assertion);
        Nunit3VerifyDiagnostic("object expected, object[] actual", assertion);
        Nunit3VerifyDiagnostic("object expected, List<object> actual", assertion);
        Nunit3VerifyDiagnostic("DateTime expected, DateTime[] actual", assertion);
        Nunit3VerifyDiagnostic("DateTime expected, List<DateTime> actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.Contains(actual, expected{0});")]
    [AssertionDiagnostic("Assert.That(actual, Has.Member(expected));")]
    [AssertionDiagnostic("Assert.That(actual, Does.Contain(expected));")]
    [AssertionDiagnostic("Assert.That(actual, Contains.Item(expected));")]
    [Implemented]
    public void Nunit4_CollectionAssertContains_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("object expected, IEnumerable actual", assertion);
        Nunit4VerifyDiagnostic("object expected, string[] actual", assertion);
        Nunit4VerifyDiagnostic("object expected, List<string> actual", assertion);
        Nunit4VerifyDiagnostic("object expected, object[] actual", assertion);
        Nunit4VerifyDiagnostic("object expected, List<object> actual", assertion);
        Nunit4VerifyDiagnostic("DateTime expected, DateTime[] actual", assertion);
        Nunit4VerifyDiagnostic("DateTime expected, List<DateTime> actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.Contains(actual, expected{0});",
        newAssertion: "actual.Should().Contain(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.Member(expected){0});",
        newAssertion: "actual.Should().Contain(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Does.Contain(expected){0});",
        newAssertion: "actual.Should().Contain(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Contains.Item(expected){0});",
        newAssertion: "actual.Should().Contain(expected{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertContains_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyNoFix("object expected, IEnumerable actual", oldAssertion);
        Nunit3VerifyFix("string expected, string[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("string expected, List<string> actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("object expected, object[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("object expected, List<object> actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("DateTime expected, DateTime[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("DateTime expected, List<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.Contains(actual, expected{0});",
        newAssertion: "actual.Should().Contain(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.Member(expected));",
        newAssertion: "actual.Should().Contain(expected);")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Does.Contain(expected));",
        newAssertion: "actual.Should().Contain(expected);")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Contains.Item(expected));",
        newAssertion: "actual.Should().Contain(expected);")]
    [Implemented]
    public void Nunit4_CollectionAssertContains_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyNoFix("object expected, IEnumerable actual", oldAssertion);
        Nunit4VerifyFix("string expected, string[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("string expected, List<string> actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("object expected, object[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("object expected, List<object> actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("DateTime expected, DateTime[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("DateTime expected, List<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, string[] actual",
        oldAssertion: "CollectionAssert.Contains(actual, expected);",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "CollectionAssert.Contains(actual, expected);",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, DateTime[] actual",
        oldAssertion: "CollectionAssert.Contains(actual, expected);",
        newAssertion: "actual.Should().Contain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<DateTime> actual",
        oldAssertion: "CollectionAssert.Contains(actual, expected);",
        newAssertion: "actual.Should().Contain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Has.Member(expected));",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Does.Contain(expected));",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Contains.Item(expected));",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [Implemented]
    public void Nunit3_CollectionAssertContains_WithCasting_TestCodeFix(string methodArguments, string oldAssertion, string newAssertion)
        => Nunit3VerifyFix(methodArguments, oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, string[] actual",
        oldAssertion: "CollectionAssert.Contains(actual, expected);",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "CollectionAssert.Contains(actual, expected);",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, DateTime[] actual",
        oldAssertion: "CollectionAssert.Contains(actual, expected);",
        newAssertion: "actual.Should().Contain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<DateTime> actual",
        oldAssertion: "CollectionAssert.Contains(actual, expected);",
        newAssertion: "actual.Should().Contain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Has.Member(expected));",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Does.Contain(expected));",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Contains.Item(expected));",
        newAssertion: "actual.Should().Contain((string)expected);")]
    [Implemented]
    public void Nunit4_CollectionAssertContains_WithCasting_TestCodeFix(string methodArguments, string oldAssertion, string newAssertion)
        => Nunit4VerifyFix(methodArguments, oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.DoesNotContain(actual, expected{0});")]
    [AssertionDiagnostic("Assert.That(actual, Has.No.Member(expected){0});")]
    [AssertionDiagnostic("Assert.That(actual, Does.Not.Contain(expected){0});")]
    [Implemented]
    public void Nunit3_CollectionAssertDoesNotContain_TestAnalyzer(string assertion)
    {
        Nunit3VerifyDiagnostic("object expected, IEnumerable actual", assertion);
        Nunit3VerifyDiagnostic("object expected, string[] actual", assertion);
        Nunit3VerifyDiagnostic("object expected, List<string> actual", assertion);
        Nunit3VerifyDiagnostic("object expected, object[] actual", assertion);
        Nunit3VerifyDiagnostic("object expected, List<object> actual", assertion);
        Nunit3VerifyDiagnostic("DateTime expected, DateTime[] actual", assertion);
        Nunit3VerifyDiagnostic("DateTime expected, List<DateTime> actual", assertion);
    }

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.DoesNotContain(actual, expected{0});")]
    [AssertionDiagnostic("Assert.That(actual, Has.No.Member(expected));")]
    [AssertionDiagnostic("Assert.That(actual, Does.Not.Contain(expected));")]
    [Implemented]
    public void Nunit4_CollectionAssertDoesNotContain_TestAnalyzer(string assertion)
    {
        Nunit4VerifyDiagnostic("object expected, IEnumerable actual", assertion);
        Nunit4VerifyDiagnostic("object expected, string[] actual", assertion);
        Nunit4VerifyDiagnostic("string expected, List<string> actual", assertion);
        Nunit4VerifyDiagnostic("object expected, object[] actual", assertion);
        Nunit4VerifyDiagnostic("object expected, List<object> actual", assertion);
        Nunit4VerifyDiagnostic("DateTime expected, DateTime[] actual", assertion);
        Nunit4VerifyDiagnostic("DateTime expected, List<DateTime> actual", assertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected{0});",
        newAssertion: "actual.Should().NotContain(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.No.Member(expected){0});",
        newAssertion: "actual.Should().NotContain(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Does.Not.Contain(expected){0});",
        newAssertion: "actual.Should().NotContain(expected{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertDoesNotContain_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit3VerifyNoFix("object expected, IEnumerable actual", oldAssertion);
        Nunit3VerifyFix("string expected, string[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("string expected, List<string> actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("object expected, object[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("object expected, List<object> actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("DateTime expected, DateTime[] actual", oldAssertion, newAssertion);
        Nunit3VerifyFix("DateTime expected, List<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected{0});",
        newAssertion: "actual.Should().NotContain(expected{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.No.Member(expected));",
        newAssertion: "actual.Should().NotContain(expected);")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Does.Not.Contain(expected));",
        newAssertion: "actual.Should().NotContain(expected);")]
    [Implemented]
    public void Nunit4_CollectionAssertDoesNotContain_TestCodeFix(string oldAssertion, string newAssertion)
    {
        Nunit4VerifyNoFix("object expected, IEnumerable actual", oldAssertion);
        Nunit4VerifyFix("string expected, string[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("string expected, List<string> actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("object expected, object[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("object expected, List<object> actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("DateTime expected, DateTime[] actual", oldAssertion, newAssertion);
        Nunit4VerifyFix("DateTime expected, List<DateTime> actual", oldAssertion, newAssertion);
    }

    [DataTestMethod]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, string[] actual",
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected);",
        newAssertion: "actual.Should().NotContain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected);",
        newAssertion: "actual.Should().NotContain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, DateTime[] actual",
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected);",
        newAssertion: "actual.Should().NotContain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<DateTime> actual",
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected);",
        newAssertion: "actual.Should().NotContain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Has.No.Member(expected));",
        newAssertion: "actual.Should().NotContain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Does.Not.Contain(expected));",
        newAssertion: "actual.Should().NotContain((string)expected);")]
    [Implemented]
    public void Nunit3_CollectionAssertDoesNotContain_WithCasting_TestCodeFix(string methodArguments, string oldAssertion, string newAssertion)
        => Nunit3VerifyFix(methodArguments, oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, string[] actual",
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected);",
        newAssertion: "actual.Should().NotContain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected);",
        newAssertion: "actual.Should().NotContain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, DateTime[] actual",
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected);",
        newAssertion: "actual.Should().NotContain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<DateTime> actual",
        oldAssertion: "CollectionAssert.DoesNotContain(actual, expected);",
        newAssertion: "actual.Should().NotContain((DateTime)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Has.No.Member(expected));",
        newAssertion: "actual.Should().NotContain((string)expected);")]
    [AssertionMethodCodeFix(
        methodArguments: "object expected, List<string> actual",
        oldAssertion: "Assert.That(actual, Does.Not.Contain(expected));",
        newAssertion: "actual.Should().NotContain((string)expected);")]
    [Implemented]
    public void Nunit4_CollectionAssertDoesNotContain_WithCasting_TestCodeFix(string methodArguments, string oldAssertion, string newAssertion)
        => Nunit4VerifyFix(methodArguments, oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(string){0});")]
    [AssertionDiagnostic("CollectionAssert.AllItemsAreInstancesOfType(actual, type{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.All.InstanceOf(typeof(int)){0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.All.InstanceOf<int>(){0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.All.InstanceOf(type){0});")]
    [AssertionDiagnostic("Assert.That(actual, Has.All.InstanceOf(typeof(int)){0});")]
    [AssertionDiagnostic("Assert.That(actual, Has.All.InstanceOf<int>(){0});")]
    [AssertionDiagnostic("Assert.That(actual, Has.All.InstanceOf(type){0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAllItemsAreInstancesOfType_TestAnalyzer(string assertion) => Nunit3VerifyDiagnostic("IEnumerable<string> actual, Type type", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(string){0});")]
    [AssertionDiagnostic("CollectionAssert.AllItemsAreInstancesOfType(actual, type{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.All.InstanceOf(typeof(int)));")]
    [AssertionDiagnostic("Assert.That(actual, Is.All.InstanceOf<int>());")]
    [AssertionDiagnostic("Assert.That(actual, Is.All.InstanceOf(type));")]
    [AssertionDiagnostic("Assert.That(actual, Has.All.InstanceOf(typeof(int)));")]
    [AssertionDiagnostic("Assert.That(actual, Has.All.InstanceOf<int>());")]
    [AssertionDiagnostic("Assert.That(actual, Has.All.InstanceOf(type));")]
    [Implemented]
    public void Nunit4_CollectionAssertAllItemsAreInstancesOfType_TestAnalyzer(string assertion) => Nunit4VerifyDiagnostic("IEnumerable<string> actual, Type type", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(string){0});",
        newAssertion: "actual.Should().AllBeOfType<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AllItemsAreInstancesOfType(actual, type{0});",
        newAssertion: "actual.Should().AllBeOfType(type{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.All.InstanceOf(typeof(int)){0});",
        newAssertion: "actual.Should().AllBeOfType<int>({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.All.InstanceOf<int>(){0});",
        newAssertion: "actual.Should().AllBeOfType<int>({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.All.InstanceOf(type){0});",
        newAssertion: "actual.Should().AllBeOfType(type{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.All.InstanceOf(typeof(int)){0});",
        newAssertion: "actual.Should().AllBeOfType<int>({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.All.InstanceOf<int>(){0});",
        newAssertion: "actual.Should().AllBeOfType<int>({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.All.InstanceOf(type){0});",
        newAssertion: "actual.Should().AllBeOfType(type{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAllItemsAreInstancesOfType_TestCodeFix(string oldAssertion, string newAssertion) => Nunit3VerifyFix("IEnumerable<string> actual, Type type", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(string){0});",
        newAssertion: "actual.Should().AllBeOfType<string>({0});")]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AllItemsAreInstancesOfType(actual, type{0});",
        newAssertion: "actual.Should().AllBeOfType(type{0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.All.InstanceOf(typeof(int)));",
        newAssertion: "actual.Should().AllBeOfType<int>();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.All.InstanceOf<int>());",
        newAssertion: "actual.Should().AllBeOfType<int>();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.All.InstanceOf(type));",
        newAssertion: "actual.Should().AllBeOfType(type);")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.All.InstanceOf(typeof(int)));",
        newAssertion: "actual.Should().AllBeOfType<int>();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.All.InstanceOf<int>());",
        newAssertion: "actual.Should().AllBeOfType<int>();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.All.InstanceOf(type));",
        newAssertion: "actual.Should().AllBeOfType(type);")]
    [Implemented]
    public void Nunit4_CollectionAssertAllItemsAreInstancesOfType_TestCodeFix(string oldAssertion, string newAssertion) => Nunit4VerifyFix("IEnumerable<string> actual, Type type", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AllItemsAreNotNull(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.All.Not.Null{0});")]
    [AssertionDiagnostic("Assert.That(actual, Has.None.Null{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAllItemsAreNotNull_TestAnalyzer(string assertion) => Nunit3VerifyDiagnostic("IEnumerable<string> actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AllItemsAreNotNull(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.All.Not.Null);")]
    [AssertionDiagnostic("Assert.That(actual, Has.None.Null);")]
    [Implemented]
    public void Nunit4_CollectionAssertAllItemsAreNotNull_TestAnalyzer(string assertion) => Nunit4VerifyDiagnostic("IEnumerable<string> actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AllItemsAreNotNull(actual{0});",
        newAssertion: "actual.Should().NotContainNulls({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.All.Not.Null{0});",
        newAssertion: "actual.Should().NotContainNulls({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.None.Null{0});",
        newAssertion: "actual.Should().NotContainNulls({0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAllItemsAreNotNull_TestCodeFix(string oldAssertion, string newAssertion) => Nunit3VerifyFix("IEnumerable<string> actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AllItemsAreNotNull(actual{0});",
        newAssertion: "actual.Should().NotContainNulls({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.All.Not.Null);",
        newAssertion: "actual.Should().NotContainNulls();")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Has.None.Null);",
        newAssertion: "actual.Should().NotContainNulls();")]
    [Implemented]
    public void Nunit4_CollectionAssertAllItemsAreNotNull_TestCodeFix(string oldAssertion, string newAssertion) => Nunit4VerifyFix("IEnumerable<string> actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AllItemsAreUnique(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Unique{0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAllItemsAreUnique_TestAnalyzer(string assertion) => Nunit3VerifyDiagnostic("IEnumerable<string> actual", assertion);

    [DataTestMethod]
    [AssertionDiagnostic("CollectionAssert.AllItemsAreUnique(actual{0});")]
    [AssertionDiagnostic("Assert.That(actual, Is.Unique);")]
    [Implemented]
    public void Nunit4_CollectionAssertAllItemsAreUnique_TestAnalyzer(string assertion) => Nunit4VerifyDiagnostic("IEnumerable<string> actual", assertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AllItemsAreUnique(actual{0});",
        newAssertion: "actual.Should().OnlyHaveUniqueItems({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Unique{0});",
        newAssertion: "actual.Should().OnlyHaveUniqueItems({0});")]
    [Implemented]
    public void Nunit3_CollectionAssertAllItemsAreUnique_TestCodeFix(string oldAssertion, string newAssertion) => Nunit3VerifyFix("IEnumerable<string> actual", oldAssertion, newAssertion);

    [DataTestMethod]
    [AssertionCodeFix(
        oldAssertion: "CollectionAssert.AllItemsAreUnique(actual{0});",
        newAssertion: "actual.Should().OnlyHaveUniqueItems({0});")]
    [AssertionCodeFix(
        oldAssertion: "Assert.That(actual, Is.Unique);",
        newAssertion: "actual.Should().OnlyHaveUniqueItems();")]
    [Implemented]
    public void Nunit4_CollectionAssertAllItemsAreUnique_TestCodeFix(string oldAssertion, string newAssertion) => Nunit4VerifyFix("IEnumerable<string> actual", oldAssertion, newAssertion);

    #endregion

    private void Nunit3VerifyDiagnostic(string methodArguments, string assertion)
        => VerifyDiagnostic(GenerateCode.Nunit3Assertion(methodArguments, assertion), PackageReference.Nunit_3_14_0);
    private void Nunit3VerifyNoDiagnostic(string methodArguments, string assertion)
        => VerifyNoDiagnostic(GenerateCode.Nunit3Assertion(methodArguments, assertion), PackageReference.Nunit_3_14_0);
    private void Nunit3VerifyFix(string methodArguments, string oldAssertion, string newAssertion)
        => VerifyFix(GenerateCode.Nunit3Assertion(methodArguments, oldAssertion), GenerateCode.Nunit3Assertion(methodArguments, newAssertion), PackageReference.Nunit_3_14_0);
    private void Nunit3VerifyNoFix(string methodArguments, string assertion)
        => VerifyNoFix(GenerateCode.Nunit3Assertion(methodArguments, assertion), PackageReference.Nunit_3_14_0);

    private void Nunit4VerifyDiagnostic(string methodArguments, string assertion)
        => VerifyDiagnostic(GenerateCode.Nunit4Assertion(methodArguments, assertion), PackageReference.Nunit_4_0_1);
    private void Nunit4VerifyNoDiagnostic(string methodArguments, string assertion)
        => VerifyNoDiagnostic(GenerateCode.Nunit4Assertion(methodArguments, assertion), PackageReference.Nunit_4_0_1);
    private void Nunit4VerifyFix(string methodArguments, string oldAssertion, string newAssertion)
        => VerifyFix(GenerateCode.Nunit4Assertion(methodArguments, oldAssertion), GenerateCode.Nunit4Assertion(methodArguments, newAssertion), PackageReference.Nunit_4_0_1);
    private void Nunit4VerifyNoFix(string methodArguments, string assertion)
        => VerifyNoFix(GenerateCode.Nunit4Assertion(methodArguments, assertion), PackageReference.Nunit_4_0_1);
    private void VerifyDiagnostic(string source, PackageReference nunit)
    {
        DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
            .WithAllAnalyzers()
            .WithSources(source)
            .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, nunit)
            .WithExpectedDiagnostics(new LegacyDiagnosticResult
            {
                Id = AssertAnalyzer.NUnitRule.Id,
                Message = AssertAnalyzer.Message,
                Locations = new DiagnosticResultLocation[]
                {
                        new("Test0.cs", 16, 13)
                },
                Severity = DiagnosticSeverity.Info
            })
        );
    }

    private void VerifyFix(string oldSource, string newSource, PackageReference nunit)
    {
        DiagnosticVerifier.VerifyFix(new CodeFixVerifierArguments()
            .WithDiagnosticAnalyzer<AssertAnalyzer>()
            .WithCodeFixProvider<NunitCodeFixProvider>()
            .WithSources(oldSource)
            .WithFixedSources(newSource)
            .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, nunit)
        );
    }
    private void VerifyNoFix(string source, PackageReference nunit)
    {
        DiagnosticVerifier.VerifyNoFix(new CodeFixVerifierArguments()
            .WithDiagnosticAnalyzer<AssertAnalyzer>()
            .WithCodeFixProvider<NunitCodeFixProvider>()
            .WithSources(source)
            .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, nunit)
        );
    }
    private void VerifyNoDiagnostic(string source, PackageReference nunit)
    {
        DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
            .WithAllAnalyzers()
            .WithSources(source)
            .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, nunit)
        );
    }
}