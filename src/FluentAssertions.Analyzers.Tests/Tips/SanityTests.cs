using FluentAssertions.Analyzers.TestUtils;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class SanityTests
    {
        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/11")]
        public void CountWithPredicate()
        {
            const string assertion = "actual.Count(d => d.Message.Contains(\"a\")).Should().Be(2);";
            var source = GenerateCode.GenericIListCodeBlockAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/10")]
        public void AssertionCallMultipleMethodWithTheSameNameAndArguments()
        {
            const string assertion = "actual.Should().Contain(d => d.Message.Contains(\"a\")).And.Contain(d => d.Message.Contains(\"c\"));";
            var source = GenerateCode.GenericIListCodeBlockAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/44")]
        public void CollectionShouldHaveElementAt_ShouldIgnoreDictionaryTypes()
        {
            string source = GenerateCode.GenericIDictionaryAssertion("actual[\"key\"].Should().Be(expectedValue);");
            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/13")]
        public void PropertyOfIndexerShouldBe_ShouldNotThrowException()
        {
            const string assertion = "actual[0].Message.Should().Be(\"test\");";
            var source = GenerateCode.GenericIListCodeBlockAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/13")]
        public void PropertyOfElementAtShouldBe_ShouldNotTriggerDiagnostic()
        {
            const string assertion = "actual.ElementAt(0).Message.Should().Be(\"test\");";
            var source = GenerateCode.GenericIListCodeBlockAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/10")]
        public void NestedAssertions_ShouldNotTrigger()
        {
            const string declaration = "var nestedList = new List<List<int>>();";
            const string assertion = "nestedList.Should().NotBeNull().And.ContainSingle().Which.Should().NotBeEmpty();";
            var source = GenerateCode.GenericIListCodeBlockAssertion(declaration + assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/18")]
        public void DictionaryShouldContainPair_WhenPropertiesOfDifferentVariables_ShouldNotTrigger()
        {
            const string assertion = "actual.Should().ContainValue(pair.Value).And.ContainKey(otherPair.Key);";
            var source = GenerateCode.GenericIDictionaryAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/41")]
        public void ExpressionBasedFunction_ShouldNotThrow()
        {
            const string source = @"
public class TestClass
{
    private SomeClass CreateSomeClass() => new SomeClass();
 
    public class SomeClass
    { }
    public static void Main() { }
}";

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/58")]
        public void StaticWithNameof_ShouldNotThrow()
        {
            const string source = @"public class TestClass
{
    private static string StaticResult { get; set; }

    public static void Main()
    {
        StaticResult = nameof(Main);
    }
}";
            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/49")]
        public void WritingToConsole_ShouldNotThrow()
        {
            const string source = @"
public class TestClass
{
    public static void Main()
    {
        System.Console.WriteLine();
    }
}";

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/63")]
        public void Collection_SelectWhereShouldOnlyHaveUniqueItems_ShouldNotTrigger()
        {
            const string source = @"
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace TestNamespace
{
    public class Program
    {
        public static void Main()
        {
            var list = new[] { 1, 2, 3 };
    
            list.Select(e => e.ToString())
                .Where(e => e != string.Empty)
                .Should()
                .OnlyHaveUniqueItems();
        }
    }
}";

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/63")]
        public void StringShouldNotBeEmptyAndShouldNotBeNull_ShouldNotTrigger()
        {
            const string assertion = "actual.Should().NotBeEmpty().And.Subject.Should().NotBeNull();";
            var source = GenerateCode.StringAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/65")]
        public void CustomClass_ShouldNotTrigger_DictionaryAnalyzers()
        {
            const string source = @"
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace TestNamespace
{
    class MyDict<TKey, TValue>
    {
        public bool ContainsKey(TKey key) => false;
    }
    
    public class Program
    {
        public static void Main()
        {
            var dict = new MyDict<int, string>();
            dict.ContainsKey(0).Should().BeTrue();
        }
    }
}";

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/64")]
        public void CollectionShouldNotContainProperty_WhenAssertionIsIdiomatic_ShouldNotTrigger()
        {
            const string source = @"
using FluentAssertions;
using FluentAssertions.Extensions;

public class TestClass
{
    public static void Main()
    {
        var list = new[] { string.Empty };
        list.Should().OnlyContain(e => e.Contains(string.Empty));
    }
}";

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/66")]
        public void CollectionShouldHaveElementAt_ShouldNotThrow()
        {
            const string source = @"
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace TestNamespace
{
    public class Program
    {
        public static void Main()
        {
            var list = new[] { "" FOO "" };
            list[0].Trim().Should().Be(""FOO"");
        }
    }
}";

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/172")]
        public void AssertAreEqualDoesNotCompile()
        {
            const string oldSource = @"
using FluentAssertions;
using FluentAssertions.Extensions;

namespace TestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Program
    {
        public static void Main()
        {
            double x = 5;

            Assert.AreEqual(1, (int)x);
        }
    }
}";
            const string newSource = @"
using FluentAssertions;
using FluentAssertions.Extensions;

namespace TestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Program
    {
        public static void Main()
        {
            double x = 5;

            ((int)x).Should().Be(1);
        }
    }
}";

            DiagnosticVerifier.VerifyFix(new CodeFixVerifierArguments()
                .WithCodeFixProvider<MsTestCodeFixProvider>()
                .WithDiagnosticAnalyzer<AssertAnalyzer>()
                .WithSources(oldSource)
                .WithFixedSources(newSource)
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0, PackageReference.MSTestTestFramework_3_1_1)
            );
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/163")]
        public void GlobalUsingsWithAssert()
        {
            const string globalUsings = @"
global using Xunit;
global using FluentAssertions;
global using FluentAssertions.Extensions;";
            const string source = @"
public class TestClass
{
    public static void Main()
    {
        var x = false;
        Assert.True(x);
        TestClass.True(x);
    }

    public static void True(bool input)
    {
    }
}";

            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithSources(source, globalUsings)
                .WithAllAnalyzers()
                .WithPackageReferences(PackageReference.XunitAssert_2_5_1, PackageReference.FluentAssertions_6_12_0)
                .WithExpectedDiagnostics(new LegacyDiagnosticResult()
                {
                    Id = AssertAnalyzer.XunitRule.Id,
                    Message = AssertAnalyzer.Message,
                    Severity = DiagnosticSeverity.Info,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 7, 9) }
                })
            );
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/207")]
        public void PropertiesOfTypes()
        {
            const string source = @"
using FluentAssertions;
using FluentAssertions.Extensions;
using System.Collections.Generic;
using System.Linq;
public class TestClass
{
    public static void Main()
    {
        var x = new TestType1();
        x.Prop1.Prop2.List.Any().Should().BeTrue();
        x.Prop1.Prop2.Count.Should().BeGreaterThan(10);
    }
}

public class TestType1
{
    public TestType2 Prop1 { get; set; }
}
public class TestType2
{
    public TestType3 Prop2 { get; set; }
}
public class TestType3
{
    public List<int> List { get; set; }
    public int Count { get; set; }
}";

            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithSources(source)
                .WithAllAnalyzers()
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
                .WithExpectedDiagnostics(new LegacyDiagnosticResult()
                {
                    Id = FluentAssertionsAnalyzer.DiagnosticId,
                    Message = DiagnosticMetadata.CollectionShouldNotBeEmpty_AnyShouldBeTrue.Message,
                    Severity = DiagnosticSeverity.Info,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 11, 9) }
                })
            );
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/215")]
        public void ShouldNotFailToAnalyze1()
        {
            const string source = @"
#nullable enable
using FluentAssertions;
using FluentAssertions.Extensions;
using System;
public class TestClass
{
    public static void Main()
    {
        var otherComponent = new OtherComponent();
        otherComponent.Name.Should().Be(""SomeOtherComponent"");
        otherComponent.Version.Should().Be(""1.2.3"");
        otherComponent.DownloadUrl.Should().Be(new Uri(""https://sampleurl.com""));
        otherComponent.Hash.Should().Be(""SampleHash"");
    }
}

public class OtherComponent
{
    public string? Name { get; set; }

    public string? Version { get; set; }

    public Uri? DownloadUrl { get; set; }

    public string? Hash { get; set; }
}";

            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithSources(source)
                .WithAllAnalyzers()
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
            );
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/276")]
        public void ShouldNotFailToAnalyze2()
        {
            const string source = @"
#nullable enable            
using FluentAssertions;
using FluentAssertions.Extensions;
using System.Linq;
using System.Collections.Generic;

public class TestClass
{
    public static void Main()
    {
        var response = new MyResponse();
        response.MyCollection?.Count().Should().Be(2);
    }
}

public class MyResponse
{
    public IEnumerable<MyCollectionType>? MyCollection { get; set; }
}
public class MyCollectionType { }";

            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithSources(source)
                .WithAllAnalyzers()
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
                .WithExpectedDiagnostics(new LegacyDiagnosticResult()
                {
                    Id = FluentAssertionsAnalyzer.DiagnosticId,
                    Message = DiagnosticMetadata.NullConditionalMayNotExecute.Message,
                    Severity = DiagnosticSeverity.Info, // TODO: change to warning
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 13, 9) }
                })
            );
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/290")]
        public void ShouldNotReportIssue290()
        {
            const string source = @"
using FluentAssertions;
using FluentAssertions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
public class TestClass
{
    public static void Main()
    {
        IEnumerable<Item> expectedOrderedNames = new[] { new Item(""Alpha""), new Item(""Bravo""), new Item(""Charlie"") };
        IEnumerable<Parent> actual = GetSortedItems();

        actual.Select(x => x.Item).Should().Equal(expectedOrderedNames);
    }

    static IEnumerable<Parent> GetSortedItems()
    {
        yield return new Parent(""Bravo"");
        yield return new Parent(""Charlie"");
        yield return new Parent(""Alpha"");
    }
}

public class Item
{
    public string Name { get; set; }
    public Guid Id { get; set; }

    public Item(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
    }
}

public class Parent
{
    public Item Item { get; set; }

    public Parent(string name)
    {
        Item = new Item(name);
    }
}";

            DiagnosticVerifier.VerifyDiagnostic(new DiagnosticVerifierArguments()
                .WithSources(source)
                .WithAllAnalyzers()
                .WithPackageReferences(PackageReference.FluentAssertions_6_12_0)
            );
        }
    }
}