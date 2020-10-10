using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NUnit.Framework;
using FluentAssertions;

namespace FluentNHibernate.Testing.ConventionFinderTests
{
    [TestFixture]
    public class AddingTypeTests
    {
        private DefaultConventionFinder finder;

        [SetUp]
        public void CreateFinder()
        {
            finder = new DefaultConventionFinder();
        }

        [Test]
        public void AddingSingleShouldntThrowIfHasParameterlessConstructor()
        {
            Action act = () => finder.Add<ConventionWithParameterlessConstructor>();

            act.Should().NotThrow();
        }

        [Test]
        public void AddingSingleShouldntThrowIfHasIConventionFinderConstructor()
        {
            Action act = () => finder.Add<ConventionWithIConventionFinderConstructor>();

            act.Should().NotThrow();

        }

        [Test]
        public void AddingSingleShouldThrowIfNoParameterlessConstructor()
        {
            Action act = () => finder.Add<ConventionWithoutValidConstructor>();

            act.Should().Throw<MissingConstructorException>();
        }

        [Test]
        public void AddingSingleShouldThrowIfNoIConventionFinderConstructor()
        {
            Action act = () => finder.Add<ConventionWithoutValidConstructor>();

            act.Should().Throw<MissingConstructorException>();
        }

        [Test]
        public void AddingAssemblyShouldntThrowIfNoIConventionFinderConstructor()
        {
            Action act = () => finder.AddAssembly(typeof(ConventionWithoutValidConstructor).Assembly);

            act.Should().NotThrow();
        }
    }

    public class ConventionWithParameterlessConstructor : IClassConvention
    {
        public ConventionWithParameterlessConstructor()
        { }

        public void Apply(IClassInstance instance) {}
    }

    public class ConventionWithIConventionFinderConstructor : IClassConvention
    {
        public ConventionWithIConventionFinderConstructor(IConventionFinder conventionFinder)
        { }

        public void Apply(IClassInstance instance) {}
    }

    public class ConventionWithoutValidConstructor : IClassConvention
    {
        public ConventionWithoutValidConstructor(int someParameter)
        { }

        public void Apply(IClassInstance instance) {}
    }
}