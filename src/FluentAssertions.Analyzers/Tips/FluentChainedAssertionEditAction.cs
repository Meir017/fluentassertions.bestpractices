using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Operations;

namespace FluentAssertions.Analyzers;

public record struct FluentChainedAssertionEditActionContext(
    IInvocationOperation AssertionA,
    InvocationExpressionSyntax AssertionAExpression,
    IPropertyReferenceOperation AndOrWhich,
    IInvocationOperation AssertionB,
    InvocationExpressionSyntax AssertionBExpression,
    IInvocationOperation Should,
    IOperation Subject
);

public interface IFluentChainedAssertionEditAction
{
    void Apply(DocumentEditor editor, FluentChainedAssertionEditActionContext context);
}

public static class FluentChainedAssertionEditAction
{
    public static IFluentChainedAssertionEditAction CombineAssertionsWithName(string newName) => new CombineAssertionsWithNameEditAction(newName);

    private class CombineAssertionsWithNameEditAction(string newName) : IFluentChainedAssertionEditAction
    {
        public void Apply(DocumentEditor editor, FluentChainedAssertionEditActionContext context)
        {
            var newNameNode = (IdentifierNameSyntax)editor.Generator.IdentifierName(newName);
            
            var assertionMemberAccess = (MemberAccessExpressionSyntax)context.AssertionAExpression.Expression;
            var allArguments = SyntaxFactory.ArgumentList()
                .AddArguments([.. context.AssertionAExpression.ArgumentList.Arguments])
                .AddArguments([.. context.AssertionBExpression.ArgumentList.Arguments]);
            var newAssertion = context.AssertionAExpression
                .WithExpression(assertionMemberAccess.WithName(newNameNode))
                .WithArgumentList(allArguments);

            editor.ReplaceNode(context.AssertionBExpression, newAssertion);
        }
    }
}