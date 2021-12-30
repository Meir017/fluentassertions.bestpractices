﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("{Name}")]
    public class MemberValidator
    {
        private readonly Func<SeparatedSyntaxList<ArgumentSyntax>, bool> _argumentsPredicate;

        public string Name { get; }

        public MemberValidator(string name)
        {
            Name = name;
            _argumentsPredicate = _ => true;
        }

        public MemberValidator(string name, Func<SeparatedSyntaxList<ArgumentSyntax>, bool> argumentsPredicate)
        {
            Name = name;
            _argumentsPredicate = argumentsPredicate;
        }

        public bool MatchesInvocationExpression(InvocationExpressionSyntax invocation)
        {
            return _argumentsPredicate(invocation.ArgumentList.Arguments);
        }

        public bool MatchesElementAccessExpression(ElementAccessExpressionSyntax elementAccess)
        {
            return _argumentsPredicate(elementAccess.ArgumentList.Arguments);
        }

        public static MemberValidator And { get; } = new MemberValidator(nameof(And));
        public static MemberValidator Which { get; } = new MemberValidator(nameof(Which));
        public static MemberValidator Should { get; } = new MemberValidator(nameof(Should));

        public static MemberValidator MathodNotContainingLambda(string name) => new MemberValidator(name, MethodNotContainingLambdaPredicate);
        public static MemberValidator MathodContainingLambda(string name) => new MemberValidator(name, MethodContainingLambdaPredicate);
        public static MemberValidator ArgumentIsVariable(string name) => new MemberValidator(name, ArgumentIsVariablePredicate);
        public static MemberValidator ArgumentIsLiteral<T>(string name, T value) => new MemberValidator(name, arguments => ArgumentIsLiteralPredicate(arguments, value));
        public static MemberValidator ArgumentIsIdentifierOrLiteral(string name) => new MemberValidator(name, ArgumentIsIdentifierOrLiteralPredicate);
        public static MemberValidator HasArguments(string name) => new MemberValidator(name, arguments => arguments.Any());
        public static MemberValidator HasNoArguments(string name) => new MemberValidator(name, arguments => !arguments.Any());
        public static MemberValidator ArgumentsMatch(string name, params Predicate<ArgumentSyntax>[] predicates) => new MemberValidator(name, arguments => ArgumentsMatchPredicate(arguments, predicates));

        public static bool MethodNotContainingLambdaPredicate(SeparatedSyntaxList<ArgumentSyntax> arguments)
        {
            if (!arguments.Any()) return true;

            return !(arguments.First().Expression is LambdaExpressionSyntax);
        }
        public static bool MethodContainingLambdaPredicate(SeparatedSyntaxList<ArgumentSyntax> arguments)
        {
            if (!arguments.Any()) return false;

            return arguments.First().Expression is LambdaExpressionSyntax;
        }
        public static bool ArgumentIsVariablePredicate(SeparatedSyntaxList<ArgumentSyntax> arguments)
        {
            if (!arguments.Any()) return false;

            return ArgumentIsVariablePredicate(arguments.First());
        }
        public static bool ArgumentIsVariablePredicate(ArgumentSyntax argument)
        {
            if (argument.Expression is IdentifierNameSyntax identifier)
            {
                var argumentName = identifier.Identifier.Text;

                var variableName = VariableNameExtractor.ExtractVariabeName(argument);

                return variableName == argumentName;
            }
            return false;
        }
        public static bool ArgumentIsLiteralPredicate<T>(SeparatedSyntaxList<ArgumentSyntax> arguments, T value)
        {
            return arguments.Any()
                && arguments.First().Expression is LiteralExpressionSyntax literal
                && literal.Token.Value is T argument
                && argument.Equals(value);
        }
        public static bool ArgumentIsIdentifierOrLiteralPredicate(SeparatedSyntaxList<ArgumentSyntax> arguments)
        {
            if (!arguments.Any()) return false;

            var argumentsExpression = arguments.First().Expression;
            return argumentsExpression is IdentifierNameSyntax || argumentsExpression is LiteralExpressionSyntax;
        }
        public static bool ArgumentsMatchPredicate(SeparatedSyntaxList<ArgumentSyntax> arguments, Predicate<ArgumentSyntax>[] predicates)
        {
            if (arguments.Count < predicates.Length) return false;

            for (int i = 0; i < predicates.Length; i++)
            {
                if (!predicates[i](arguments[i])) return false;
            }

            return true;
        }
    }

    public class ArgumentValidator
    {
        public static Func<ArgumentSyntax, bool> IsType(ITypeSymbol type) => null;
    }
}