using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StringAssertStartsWithAnalyzer : MsTestStringAssertAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.MsTest.StringAssertStartsWith;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().StartWith() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new StringAssertStartsWithSyntaxVisitor();
            }
        }

		public class StringAssertStartsWithSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
		{
			public StringAssertStartsWithSyntaxVisitor() : base(new MemberValidator("StartsWith"))
            {
            }
		}
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StringAssertStartsWithCodeFix)), Shared]
    public class StringAssertStartsWithCodeFix : MsTestCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(StringAssertStartsWithAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return RenameMethodAndReorderActualExpectedAndReplaceWithSubjectShould(expression, "StartsWith", "StartWith", "StringAssert");
		}
    }
}