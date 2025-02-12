using System.Reflection;

using NUnit.Framework;

namespace AslHelp.Shared.Tests;

public class Tests
{
    // [SetUp]
    // public void Setup() { }

    [Test]
    public void Test1()
    {
        const string Format = "Name: {Name}";
        const string Expected = "Name: John";

        var obj = new TestData("John");

        var result = PropertyExpressionFormatter.Format(Format, obj, BindingFlags.Public | BindingFlags.Instance);
        Assert.That(result, Is.EqualTo(Expected));
    }
}

file sealed record TestData(
    string Name);
