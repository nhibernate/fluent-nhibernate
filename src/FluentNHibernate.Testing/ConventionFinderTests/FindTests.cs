using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionFinderTests
{
    //[TestFixture]
    //public class FindTests
    //{
    //    private DefaultConventionFinder finder;

    //    [SetUp]
    //    public void CreateConventionFinder()
    //    {
    //        finder = new DefaultConventionFinder();
    //    }

    //    [Test]
    //    public void ShouldFindNothingWhenNoAssembliesGiven()
    //    {
    //        finder.Find<IEntireMappingsConvention>()
    //            .ShouldBeEmpty();
    //    }

    //    [Test]
    //    public void ShouldFindTypesFromAssembly()
    //    {
    //        finder.AddAssembly(GetType().Assembly);
    //        finder.Find<IEntireMappingsConvention>()
    //            .ShouldContain(c => c is DummyAssemblyConvention);
    //    }

    //    [Test]
    //    public void ShouldFindTypesThatHaveConstructorNeedingFinder()
    //    {
    //        finder.AddAssembly(GetType().Assembly);
    //        finder.Find<IEntireMappingsConvention>()
    //            .ShouldContain(c => c is DummyFinderAssemblyConvention);
    //    }

    //    [Test]
    //    public void ShouldntFindGenericTypes()
    //    {
    //        finder.AddAssembly(GetType().Assembly);
    //        finder.Find<IEntireMappingsConvention>()
    //            .ShouldNotContain(c => c.GetType() == typeof(OpenGenericAssemblyConvention<>));
    //    }

    //    [Test]
    //    public void ShouldOnlyFindExplicitAdded()
    //    {
    //        finder.Add<DummyAssemblyConvention>();
    //        finder.Find<IEntireMappingsConvention>()
    //            .ShouldHaveCount(1)
    //            .ShouldContain(c => c is DummyAssemblyConvention);
    //    }

    //    [Test]
    //    public void ShouldOnlyAddInstanceOnceIfHasMultipleInterfaces()
    //    {
    //        finder.Add<MultiPartConvention>();
    //        var ids = finder.Find<IIdConvention>();
    //        var properties = finder.Find<IPropertyConvention>();

    //        ids.ShouldHaveCount(1);
    //        properties.ShouldHaveCount(1);

    //        ids.First().ShouldEqual(properties.First());
    //    }

    //    [Test]
    //    public void ShouldOnlyAddInstanceOnce()
    //    {
    //        finder.Add<DummyAssemblyConvention>();
    //        finder.Add<DummyAssemblyConvention>();
    //        var conventions = finder.Find<IEntireMappingsConvention>();

    //        conventions.ShouldHaveCount(1);
    //    }

    //    [Test]
    //    public void ShouldAllowAddingInstanceMultipleTimesIfHasMultipleAttribute()
    //    {
    //        finder.Add<DummyAssemblyWithMultipleAttributeConvention>();
    //        finder.Add<DummyAssemblyWithMultipleAttributeConvention>();
    //        var conventions = finder.Find<IEntireMappingsConvention>();

    //        conventions.ShouldHaveCount(2);
    //    }

    //    [Test]
    //    public void ShouldOnlyAddInstanceOnceIfHasMultipleInterfacesWhenAddedByAssembly()
    //    {
    //        finder.AddAssembly(typeof(MultiPartConvention).Assembly);
    //        var ids = finder.Find<IIdConvention>().Where(c => c.GetType() == typeof(MultiPartConvention));
    //        var properties = finder.Find<IPropertyConvention>().Where(c => c.GetType() == typeof(MultiPartConvention));

    //        ids.ShouldHaveCount(1);
    //        properties.ShouldHaveCount(1);

    //        ids.First().ShouldEqual(properties.First());
    //    }
    //}

    //public class OpenGenericAssemblyConvention<T> : IEntireMappingsConvention
    //{
    //    public bool Accept(IEnumerable<IClassMap> target)
    //    {
    //        return false;
    //    }

    //    public void Apply(IEnumerable<IClassMap> target)
    //    {
    //    }
    //}

    //public class DummyAssemblyConvention : IEntireMappingsConvention
    //{
    //    public bool Accept(IEnumerable<IClassMap> target)
    //    {
    //        return false;
    //    }

    //    public void Apply(IEnumerable<IClassMap> target)
    //    {
    //    }
    //}

    //[Multiple]
    //public class DummyAssemblyWithMultipleAttributeConvention : IEntireMappingsConvention
    //{
    //    public bool Accept(IEnumerable<IClassMap> target)
    //    {
    //        return false;
    //    }

    //    public void Apply(IEnumerable<IClassMap> target)
    //    {
    //    }
    //}

    //public class DummyFinderAssemblyConvention : IEntireMappingsConvention
    //{
    //    public DummyFinderAssemblyConvention(IConventionFinder finder)
    //    {

    //    }

    //    public bool Accept(IEnumerable<IClassMap> target)
    //    {
    //        return false;
    //    }

    //    public void Apply(IEnumerable<IClassMap> target)
    //    {
    //    }
    //}

    //public class MultiPartConvention : IIdConvention, IPropertyConvention
    //{
    //    public bool Accept(IIdentityPart target)
    //    {
    //        return false;
    //    }

    //    public void Apply(IIdentityPart target)
    //    {
    //    }

    //    public bool Accept(IProperty target)
    //    {
    //        return false;
    //    }

    //    public void Apply(IProperty target)
    //    {
    //    }
    //}
}
