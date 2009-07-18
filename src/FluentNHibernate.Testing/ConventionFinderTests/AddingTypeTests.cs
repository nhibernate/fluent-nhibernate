using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using NUnit.Framework;

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
            var ex = Catch.Exception(() => finder.Add<ConventionWithParameterlessConstructor>());

            ex.ShouldBeNull();
        }

        [Test]
        public void AddingSingleShouldntThrowIfHasIConventionFinderConstructor()
        {
            var ex = Catch.Exception(() => finder.Add<ConventionWithIConventionFinderConstructor>());

            ex.ShouldBeNull();
        }

        [Test]
        public void AddingSingleShouldThrowIfNoParameterlessConstructor()
        {
            var ex = Catch.Exception(() => finder.Add<ConventionWithoutValidConstructor>());

            ex.ShouldBeOfType<MissingConstructorException>();
            ex.ShouldNotBeNull();
        }

        [Test]
        public void AddingSingleShouldThrowIfNoIConventionFinderConstructor()
        {
            var ex = Catch.Exception(() => finder.Add<ConventionWithoutValidConstructor>());

            ex.ShouldBeOfType<MissingConstructorException>();
            ex.ShouldNotBeNull();
        }

        [Test]
        public void AddingAssemblyShouldntThrowIfNoIConventionFinderConstructor()
        {
            var ex = Catch.Exception(() => finder.AddAssembly(typeof(ConventionWithoutValidConstructor).Assembly));

            ex.ShouldBeNull();
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