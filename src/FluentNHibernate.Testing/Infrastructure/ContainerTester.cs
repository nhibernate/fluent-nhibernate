using System;
using FluentNHibernate.Infrastructure;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Infrastructure;

[TestFixture]
public class ContainerTester
{
    private Container container;

    [SetUp]
    public void CreateContainer()
    {
        container = new Container();
    }

    [Test]
    public void ShouldResolveRegisteredType()
    {
        container.Register<IExample>(c => new Example());

        container.Resolve<IExample>()
            .ShouldNotBeNull()
            .ShouldBeOfType<Example>();
    }

    [Test]
    public void ShouldThrowExceptionWhenResolvingUnregisteredType()
    {
        Action act = () => container.Resolve<IExample>();

        act.ShouldThrow<ResolveException>()
            .WithMessage($"Unable to resolve dependency: '{typeof(IExample).FullName}'");
    }

    private interface IExample
    {}

    private class Example : IExample
    {}
}
