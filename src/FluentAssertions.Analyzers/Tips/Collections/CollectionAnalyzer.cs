﻿using Microsoft.CodeAnalysis;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    public abstract class CollectionAnalyzer : FluentAssertionsAnalyzer
    {
        protected override bool ShouldAnalyzeVariableType(ITypeSymbol type, SemanticModel semanticModel)
        {
            return type.Name != "String"
                && type.AllInterfaces.Any(@interface => @interface.Name == "IEnumerable")
                && !type.AllInterfaces.Any(@interface => @interface.Name == "IDictionary");
        }
    }
}
