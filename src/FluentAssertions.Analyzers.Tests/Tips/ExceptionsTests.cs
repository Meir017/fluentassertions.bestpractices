using FluentAssertions.Analyzers.TestUtils;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class ExceptionsTests
    {
        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().Be(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().Be(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_ShouldThrowAndMessageShouldBe_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithMessage_ShouldThrowAndMessageShouldBe);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().Contain(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().Contain(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_ShouldThrowAndMessageShouldContain_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithMessage_ShouldThrowAndMessageShouldContain);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().EndWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().EndWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_ShouldThrowAndMessageShouldEndWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithMessage_ShouldThrowAndMessageShouldEndWith);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().StartWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().StartWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_ShouldThrowAndMessageShouldStartWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithMessage_ShouldThrowAndMessageShouldStartWith);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().Be(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().Be(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_ShouldThrowWhichMessageShouldBe_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithMessage_ShouldThrowWhichMessageShouldBe);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().Contain(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().Contain(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_ShouldThrowWhichMessageShouldContain_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithMessage_ShouldThrowWhichMessageShouldContain);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().EndWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().EndWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_ShouldThrowWhichMessageShouldEndWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithMessage_ShouldThrowWhichMessageShouldEndWith);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().StartWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().StartWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_ShouldThrowWhichMessageShouldStartWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithMessage_ShouldThrowWhichMessageShouldStartWith);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().Contain(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"*{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().Contain(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"*{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().Contain(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"*a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().Contain(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"*a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().Be(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(expectedMessage{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().Be(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(expectedMessage{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().Be(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().Be(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().StartWith(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().StartWith(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().StartWith(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().StartWith(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().EndWith(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"*{{expectedMessage}}\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().EndWith(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"*{{expectedMessage}}\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().EndWith(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"*a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().EndWith(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"*a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().Be(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().Be(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyAndMessageShouldBe_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyAndMessageShouldBe);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().Contain(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().Contain(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyAndMessageShouldContain_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyAndMessageShouldContain);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().EndWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().EndWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyAndMessageShouldEndWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyAndMessageShouldEndWith);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().StartWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().StartWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyAndMessageShouldStartWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyAndMessageShouldStartWith);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().Be(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().Be(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyWhichMessageShouldBe_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyWhichMessageShouldBe);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().Contain(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().Contain(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyWhichMessageShouldContain_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyWhichMessageShouldContain);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().EndWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().EndWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyWhichMessageShouldEndWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyWhichMessageShouldEndWith);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().StartWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().StartWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyWhichMessageShouldStartWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithMessage_ShouldThrowExactlyWhichMessageShouldStartWith);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().Contain(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"*{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().Contain(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"*{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().Contain(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"*a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().Contain(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"*a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().Be(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(expectedMessage{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().Be(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(expectedMessage{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().Be(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().Be(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().StartWith(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().StartWith(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().StartWith(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().StartWith(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().EndWith(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"*{{expectedMessage}}\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().EndWith(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"*{{expectedMessage}}\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().EndWith(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"*a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().EndWith(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"*a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.InnerException.Should().BeOfType<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithInnerException_ShouldThrowExactlyAndInnerExceptionShouldBeOfType_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithInnerException_ShouldThrowExactlyAndInnerExceptionShouldBeOfType);
        
        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.InnerException.Should().BeOfType<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithInnerException_ShouldThrowExactlyWhichInnerExceptionShouldBeOfType_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithInnerException_ShouldThrowExactlyWhichInnerExceptionShouldBeOfType);
        
        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.InnerException.Should().BeOfType<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowWithInnerException_ShouldThrowAndInnerExceptionShouldBeOfType_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithInnerException_ShouldThrowAndInnerExceptionShouldBeOfType);
        
        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.InnerException.Should().BeOfType<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowWithInnerException_ShouldThrowWhichInnerExceptionShouldBeOfType_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithInnerException_ShouldThrowWhichInnerExceptionShouldBeOfType);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.InnerException.Should().BeOfType<ArgumentException>({0});",
            newAssertion: "action.Should().Throw<Exception>().WithInnerExceptionExactly<ArgumentException>({0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.InnerException.Should().BeOfType<ArgumentException>({0});",
            newAssertion: "action.Should().Throw<Exception>().WithInnerExceptionExactly<ArgumentException>({0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.InnerException.Should().BeOfType<ArgumentException>({0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithInnerExceptionExactly<ArgumentException>({0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.InnerException.Should().BeOfType<ArgumentException>({0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithInnerExceptionExactly<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowWithInnerExceptionExactly_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.InnerException.Should().BeAssignableTo<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithInnerException_ShouldThrowExactlyAndInnerExceptionShouldBeAssignableTo_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithInnerException_ShouldThrowExactlyAndInnerExceptionShouldBeAssignableTo);
        
        [DataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.InnerException.Should().BeAssignableTo<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithInnerException_ShouldThrowExactlyWhichInnerExceptionShouldBeAssignableTo_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowExactlyWithInnerException_ShouldThrowExactlyWhichInnerExceptionShouldBeAssignableTo);
        
        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.InnerException.Should().BeAssignableTo<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowWithInnerException_ShouldThrowAndInnerExceptionShouldBeAssignableTo_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithInnerException_ShouldThrowAndInnerExceptionShouldBeAssignableTo);
        
        [DataTestMethod]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.InnerException.Should().BeAssignableTo<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowWithInnerException_ShouldThrowWhichInnerExceptionShouldBeAssignableTo_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion, DiagnosticMetadata.ExceptionShouldThrowWithInnerException_ShouldThrowWhichInnerExceptionShouldBeAssignableTo);

        [DataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.InnerException.Should().BeAssignableTo<ArgumentException>({0});",
            newAssertion: "action.Should().Throw<Exception>().WithInnerException<ArgumentException>({0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.InnerException.Should().BeAssignableTo<ArgumentException>({0});",
            newAssertion: "action.Should().Throw<Exception>().WithInnerException<ArgumentException>({0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.InnerException.Should().BeAssignableTo<ArgumentException>({0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithInnerException<ArgumentException>({0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.InnerException.Should().BeAssignableTo<ArgumentException>({0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithInnerException<ArgumentException>({0});")]
        [Implemented]
        public void ExceptionShouldThrowWithInnerException_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix(oldAssertion, newAssertion);

        private void VerifyCSharpDiagnostic(string sourceAssertion, DiagnosticMetadata metadata)
        {
            var source = GenerateCode.ExceptionAssertion(sourceAssertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new LegacyDiagnosticResult
            {
                Id = FluentAssertionsAnalyzer.DiagnosticId,
                Message = metadata.Message,
                VisitorName = metadata.Name,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 9,13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpFix(string oldSourceAssertion, string newSourceAssertion)
        {
            var oldSource = GenerateCode.ExceptionAssertion(oldSourceAssertion);
            var newSource = GenerateCode.ExceptionAssertion(newSourceAssertion);

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
