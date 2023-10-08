using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CollectionAssertAreNotEquivalentAnalyzer : MsTestCollectionAssertAnalyzer
{
    public const string DiagnosticId = Constants.Tips.MsTest.CollectionAssertAreNotEquivalent;
    public const string Category = Constants.Tips.Category;

    public const string Message = "Use .Should().NotBeEquivalentTo() instead.";

    protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
    protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
    {
        get
        {
            yield return new CollectionAssertAreNotEquivalentSyntaxVisitor();
        }
    }

		public class CollectionAssertAreNotEquivalentSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
		{
			public CollectionAssertAreNotEquivalentSyntaxVisitor() : base(new MemberValidator("AreNotEquivalent"))
			{
			}
		}
}

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionAssertAreNotEquivalentCodeFix)), Shared]
public class CollectionAssertAreNotEquivalentCodeFix : MsTestCollectionAssertCodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionAssertAreNotEquivalentAnalyzer.DiagnosticId);

    protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
    {
        return RenameMethodAndReorderActualExpectedAndReplaceWithSubjectShould(expression, "AreNotEquivalent", "NotBeEquivalentTo");
    }
}