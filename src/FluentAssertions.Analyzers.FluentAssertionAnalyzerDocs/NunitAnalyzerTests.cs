using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;
using CollectionAssert = NUnit.Framework.CollectionAssert;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace FluentAssertions.Analyzers.FluentAssertionAnalyzerDocs;

[TestClass]
public class NunitAnalyzerTests
{
    [TestMethod]
    public void BooleanAssertIsTrue()
    {
        // arrange
        var flag = true;

        // old assertion:
        Assert.IsTrue(flag);
        Assert.True(flag);
        Assert.That(flag);
        Assert.That(flag, Is.True);
        Assert.That(flag, Is.Not.False);

        // new assertion:
        flag.Should().BeTrue();
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsTrue_Failure_OldAssertion_0()
    {
        // arrange
        var flag = false;

        // old assertion:
        Assert.True(flag);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsTrue_Failure_OldAssertion_1()
    {
        // arrange
        var flag = false;

        // old assertion:
        Assert.IsTrue(flag);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsTrue_Failure_OldAssertion_2()
    {
        // arrange
        var flag = false;

        // old assertion:
        Assert.That(flag);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsTrue_Failure_OldAssertion_3()
    {
        // arrange
        var flag = false;

        // old assertion:
        Assert.That(flag, Is.True);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsTrue_Failure_OldAssertion_4()
    {
        // arrange
        var flag = false;

        // old assertion:
        Assert.That(flag, Is.Not.False);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsTrue_Failure_NewAssertion()
    {
        // arrange
        var flag = false;

        // new assertion:
        flag.Should().BeTrue();
    }

    [TestMethod]
    public void BooleanAssertIsFalse()
    {
        // arrange
        var flag = false;

        // old assertion:
        Assert.IsFalse(flag);
        Assert.False(flag);
        Assert.That(flag, Is.False);
        Assert.That(flag, Is.Not.True);

        // new assertion:
        flag.Should().BeFalse();
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsFalse_Failure_OldAssertion_0()
    {
        // arrange
        var flag = true;

        // old assertion:
        Assert.False(flag);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsFalse_Failure_OldAssertion_1()
    {
        // arrange
        var flag = true;

        // old assertion:
        Assert.IsFalse(flag);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsFalse_Failure_OldAssertion_2()
    {
        // arrange
        var flag = true;

        // old assertion:
        Assert.That(flag, Is.False);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsFalse_Failure_OldAssertion_3()
    {
        // arrange
        var flag = true;

        // old assertion:
        Assert.That(flag, Is.Not.True);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void BooleanAssertIsFalse_Failure_NewAssertion()
    {
        // arrange
        var flag = true;

        // new assertion:
        flag.Should().BeFalse();
    }

    [TestMethod]
    public void AssertNull()
    {
        // arrange
        object obj = null;

        // old assertion:
        Assert.IsNull(obj);
        Assert.Null(obj);
        Assert.That(obj, Is.Null);

        // new assertion:
        obj.Should().BeNull();
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNull_Failure_OldAssertion_0()
    {
        // arrange
        object obj = "foo";

        // old assertion:
        Assert.Null(obj);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNull_Failure_OldAssertion_1()
    {
        // arrange
        object obj = "foo";

        // old assertion:
        Assert.IsNull(obj);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNull_Failure_OldAssertion_2()
    {
        // arrange
        object obj = "foo";

        // old assertion:
        Assert.That(obj, Is.Null);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNull_Failure_NewAssertion()
    {
        // arrange
        object obj = "foo";

        // new assertion:
        obj.Should().BeNull();
    }

    [TestMethod]
    public void AssertNotNull()
    {
        // arrange
        object obj = "foo";

        // old assertion:
        Assert.IsNotNull(obj);
        Assert.NotNull(obj);
        Assert.That(obj, Is.Not.Null);

        // new assertion:
        obj.Should().NotBeNull();
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNotNull_Failure_OldAssertion_0()
    {
        // arrange
        object obj = null;

        // old assertion:
        Assert.NotNull(obj);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNotNull_Failure_OldAssertion_1()
    {
        // arrange
        object obj = null;

        // old assertion:
        Assert.IsNotNull(obj);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNotNull_Failure_OldAssertion_2()
    {
        // arrange
        object obj = null;

        // old assertion:
        Assert.That(obj, Is.Not.Null);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNotNull_Failure_NewAssertion()
    {
        // arrange
        object obj = null;

        // new assertion:
        obj.Should().NotBeNull();
    }

    [TestMethod]
    public void AssertIsEmpty()
    {
        // arrange
        var collection = new List<int>();

        // old assertion:
        Assert.IsEmpty(collection);
        Assert.That(collection, Is.Empty);
        CollectionAssert.IsEmpty(collection);

        // new assertion:
        collection.Should().BeEmpty();
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertIsEmpty_Failure_OldAssertion_0()
    {
        // arrange
        var collection = new List<int> { 1, 2, 3 };

        // old assertion:
        Assert.IsEmpty(collection);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertIsEmpty_Failure_OldAssertion_1()
    {
        // arrange
        var collection = new List<int> { 1, 2, 3 };

        // old assertion:
        Assert.That(collection, Is.Empty);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertIsEmpty_Failure_OldAssertion_2()
    {
        // arrange
        var collection = new List<int> { 1, 2, 3 };

        // old assertion:
        CollectionAssert.IsEmpty(collection);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertIsEmpty_Failure_NewAssertion()
    {
        // arrange
        var collection = new List<int> { 1, 2, 3 };

        // new assertion:
        collection.Should().BeEmpty();
    }

    [TestMethod]
    public void AssertIsNotEmpty()
    {
        // arrange
        var collection = new List<int> { 1, 2, 3 };

        // old assertion:
        Assert.IsNotEmpty(collection);
        Assert.That(collection, Is.Not.Empty);
        CollectionAssert.IsNotEmpty(collection);

        // new assertion:
        collection.Should().NotBeEmpty();
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertIsNotEmpty_Failure_OldAssertion_0()
    {
        // arrange
        var collection = new List<int>();

        // old assertion:
        Assert.IsNotEmpty(collection);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertIsNotEmpty_Failure_OldAssertion_1()
    {
        // arrange
        var collection = new List<int>();

        // old assertion:
        Assert.That(collection, Is.Not.Empty);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertIsNotEmpty_Failure_OldAssertion_2()
    {
        // arrange
        var collection = new List<int>();

        // old assertion:
        CollectionAssert.IsNotEmpty(collection);
}

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertIsNotEmpty_Failure_NewAssertion()
    {
        // arrange
        var collection = new List<int>();

        // new assertion:
        collection.Should().NotBeEmpty();
    }

    [TestMethod]
    public void AssertZero()
    {
        // arrange
        var number = 0;

        // old assertion:
        Assert.Zero(number);
        Assert.That(number, Is.Zero);

        // new assertion:
        number.Should().Be(0);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertZero_Failure_OldAssertion_0()
    {
        // arrange
        var number = 1;

        // old assertion:
        Assert.Zero(number);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertZero_Failure_OldAssertion_1()
    {
        // arrange
        var number = 1;

        // old assertion:
        Assert.That(number, Is.Zero);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertZero_Failure_NewAssertion()
    {
        // arrange
        var number = 1;

        // new assertion:
        number.Should().Be(0);
    }

    [TestMethod]
    public void AssertNotZero()
    {
        // arrange
        var number = 1;

        // old assertion:
        Assert.NotZero(number);
        Assert.That(number, Is.Not.Zero);

        // new assertion:
        number.Should().NotBe(0);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNotZero_Failure_OldAssertion_0()
    {
        // arrange
        var number = 0;

        // old assertion:
        Assert.NotZero(number);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNotZero_Failure_OldAssertion_1()
    {
        // arrange
        var number = 0;

        // old assertion:
        Assert.That(number, Is.Not.Zero);
    }

    [TestMethod, ExpectedTestFrameworkException]
    public void AssertNotZero_Failure_NewAssertion()
    {
        // arrange
        var number = 0;

        // new assertion:
        number.Should().NotBe(0);
    }
}
