using NUnit.Framework;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Utils;

[TestFixture]
public class ExtensionsTests
{
    class PublicConstructor
    {
        public PublicConstructor() { }
    }

    class PrivateConstructor
    {
        PrivateConstructor() { }
    }

    class ConstructorWithArguments
    {
        ConstructorWithArguments(int number) { }
    }

    [Test]
    public void CanInitialiseClass()
    {
        var type = typeof(PublicConstructor);
        var result = type.InstantiateUsingParameterlessConstructor();

        Assert.That(result, Is.InstanceOf<PublicConstructor>());
    }

    [Test]
    public void CanInitialiseClassWithPrivateConstructor()
    {
        var type = typeof(PrivateConstructor);
        var result = type.InstantiateUsingParameterlessConstructor();

        Assert.That(result, Is.InstanceOf<PrivateConstructor>());
    }

    [Test]
    public void ClassWithoutParameterlessConstructorThrowsException()
    {
        var type = typeof(ConstructorWithArguments);
        Assert.Throws<MissingConstructorException>(() => type.InstantiateUsingParameterlessConstructor());
    }
}
