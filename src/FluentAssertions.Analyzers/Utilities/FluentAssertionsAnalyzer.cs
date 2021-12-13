﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    public abstract class FluentAssertionsAnalyzer<TCSharpSyntaxVisitor> : DiagnosticAnalyzer where TCSharpSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
    {
        public const string Title = "Simplify Assertion";
        protected abstract DiagnosticDescriptor Rule { get; }

        protected abstract IEnumerable<TCSharpSyntaxVisitor> Visitors { get; }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public sealed override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterCodeBlockAction(AnalyzeCodeBlock);
        }

        private void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
        {
            var method = context.CodeBlock as MethodDeclarationSyntax;
            if (method == null) return;

            if (!ShouldAnalyzeMethod(method)) return;

            if (method.Body != null)
            {
                foreach (var statement in method.Body.Statements.OfType<ExpressionStatementSyntax>())
                {
                    var diagnostic = AnalyzeExpressionSafely(statement.Expression, context.SemanticModel);
                    if (diagnostic != null)
                    {
                        context.ReportDiagnostic(diagnostic);
                    }
                }
                return;
            }
            if (method.ExpressionBody != null)
            {
                var diagnostic = AnalyzeExpressionSafely(method.ExpressionBody.Expression, context.SemanticModel);
                if (diagnostic != null)
                {
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        protected virtual bool ShouldAnalyzeMethod(MethodDeclarationSyntax method) => true;

        protected virtual bool ShouldAnalyzeVariableType(INamedTypeSymbol type, SemanticModel semanticModel) => true;

        protected virtual Diagnostic AnalyzeExpression(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            var variableNameExtractor = new VariableNameExtractor(semanticModel);
            expression.Accept(variableNameExtractor);

            if (variableNameExtractor.VariableIdentifierName == null) return null;
            var typeInfo = semanticModel.GetTypeInfo(variableNameExtractor.VariableIdentifierName);
            if (!(typeInfo.Type is INamedTypeSymbol namedType)) return null;
            if (!ShouldAnalyzeVariableType(namedType, semanticModel)) return null;

            foreach (var visitor in Visitors)
            {
                expression.Accept(visitor);

                if (visitor.IsValid(expression))
                {
                    return CreateDiagnostic(visitor, expression);
                }
            }
            return null;
        }

        protected virtual Diagnostic CreateDiagnostic(TCSharpSyntaxVisitor visitor, ExpressionSyntax expression)
        {
            var properties = visitor.ToDiagnosticProperties()
                .Add(Constants.DiagnosticProperties.Title, Title);
            var newRule = new DiagnosticDescriptor(Rule.Id, Rule.Title, Rule.MessageFormat, Rule.Category, Rule.DefaultSeverity, true, 
                helpLinkUri: properties.GetValueOrDefault(Constants.DiagnosticProperties.HelpLink));
            return Diagnostic.Create(
                descriptor: newRule,
                location: expression.GetLocation(),
                properties: properties);
        }

        private Diagnostic AnalyzeExpressionSafely(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            try
            {
                return AnalyzeExpression(expression, semanticModel);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to analyze expression in {GetType().FullName}.\n{e}");
                return null;
            }
        }
    }

    public abstract class FluentAssertionsAnalyzer : FluentAssertionsAnalyzer<FluentAssertionsCSharpSyntaxVisitor>
    {
    }
}