using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Conventions;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionFinderTests
{
    [TestFixture]
    public class DefaultConventionFinderTests
    {
        private DefaultConventionFinder finder;

        [SetUp]
        public void CreateConventionFinder()
        {
            finder = new DefaultConventionFinder();
        }

        [Test]
        public void ShouldFindNothingWhenNoAssembliesGiven()
        {
            finder.Find<IAssemblyConvention>()
                .ShouldBeEmpty();
        }

        [Test]
        public void ShouldFindTypesFromAssembly()
        {
            finder.AddAssembly(GetType().Assembly);
            finder.Find<IAssemblyConvention>()
                .ShouldContain(c => c.GetType() == typeof(DummyAssemblyConvention));
        }

        [Test]
        public void ShouldFindTypesThatHaveConstructorNeedingFinder()
        {
            finder.AddAssembly(GetType().Assembly);
            finder.Find<IAssemblyConvention>()
                .ShouldContain(c => c.GetType() == typeof(DummyFinderAssemblyConvention));
        }

        [Test]
        public void ShouldntFindGenericTypes()
        {
            finder.AddAssembly(GetType().Assembly);
            finder.Find<IAssemblyConvention>()
                .ShouldNotContain(c => c.GetType() == typeof(OpenGenericAssemblyConvention<>));
        }
    }

    public class OpenGenericAssemblyConvention<T> : IAssemblyConvention
    {
        public bool Accept(IEnumerable<IClassMap> target)
        {
            return false;
        }

        public void Apply(IEnumerable<IClassMap> target, ConventionOverrides overrides)
        {
        }
    }

    public class DummyAssemblyConvention : IAssemblyConvention
    {
        public bool Accept(IEnumerable<IClassMap> target)
        {
            return false;
        }

        public void Apply(IEnumerable<IClassMap> target, ConventionOverrides overrides)
        {
        }
    }

    public class DummyFinderAssemblyConvention : IAssemblyConvention
    {
        public DummyFinderAssemblyConvention(IConventionFinder finder)
        {
            
        }

        public bool Accept(IEnumerable<IClassMap> target)
        {
            return false;
        }

        public void Apply(IEnumerable<IClassMap> target, ConventionOverrides overrides)
        {
        }
    }
}
