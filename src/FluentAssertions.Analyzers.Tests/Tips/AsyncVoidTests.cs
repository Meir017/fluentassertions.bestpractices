using FluentAssertions.Analyzers.TestUtils;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class AsyncVoidTests
    {
        [TestMethod]
        [Implemented]
        public void AssignAsyncVoidMethodToAction_TestAnalyzer()
        {
            const string statement = "Action action = AsyncVoidMethod;";
            var source = GenerateCode.AsyncFunctionStatement(statement);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented]
        public void AssignVoidLambdaToAction_TestAnalyzer()
        {
            const string statement = "Action action = () => {};";
            var source = GenerateCode.AsyncFunctionStatement(statement);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [DataRow("Action action = async () => { await Task.CompletedTask; };")]
        [DataRow("Action action1 = async () => { await Task.CompletedTask; }, action2 = async () => { await Task.CompletedTask; };")]
        [DataRow("Action action1 = () => { }, action2 = async () => { await Task.CompletedTask; };")]
        [DataRow("Action action1 = async () => { await Task.CompletedTask; }, action2 = () => { };")]
        [DataTestMethod]
        [Implemented]
        public void AssignAsyncVoidLambdaToAction_TestAnalyzer(string assertion)
        {
            var source = GenerateCode.AsyncFunctionStatement(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new LegacyDiagnosticResult
            {
                Id = AsyncVoidAnalyzer.DiagnosticId,
                Message = AsyncVoidAnalyzer.Message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 10,13)
                },
                Severity = DiagnosticSeverity.Warning
            });
        }

        [AssertionCodeFix(
            oldAssertion: "Action action = async () => { await Task.CompletedTask; };",
            newAssertion: "Func<Task> action = async () => { await Task.CompletedTask; };")]
        [AssertionCodeFix(
            oldAssertion: "Action action1 = async () => { await Task.CompletedTask; }, action2 = async () => { await Task.CompletedTask; };",
            newAssertion: "Func<Task> action1 = async () => { await Task.CompletedTask; }, action2 = async () => { await Task.CompletedTask; };")]
        [DataTestMethod]
        [NotImplemented]
        public void AssignAsyncVoidLambdaToAction_TestCodeFix(string oldAssertion, string newAssertion)
        {
            // no-op
        }
    }
}
