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
    public class CollectionAssertAreEqualAnalyzer : MsTestCollectionAssertAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.MsTest.CollectionAssertAreEqual;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().Equal() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CollectionAssertAreEqualSyntaxVisitor();
            }
        }

		public class CollectionAssertAreEqualSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
		{
			public CollectionAssertAreEqualSyntaxVisitor() : base(new MemberValidator("AreEqual"))
			{
			}
		}
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionAssertAreEqualCodeFix)), Shared]
    public class CollectionAssertAreEqualCodeFix : MsTestCollectionAssertCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionAssertAreEqualAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return RenameMethodAndReorderActualExpectedAndReplaceWithSubjectShould(expression, "AreEqual", "Equal");
		}
    }
}