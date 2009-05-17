using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions;
//using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class UserDefinedConventionsTester
    {
        //[Test]
        //public void ShouldApplyUserConventionsBeforeDefaults()
        //{
        //    new MappingTester<ExampleClass>()
        //        .Conventions(x => x.Add<ExampleClassTableConvention>())
        //        .ForMapping(m => { })
        //        .Element("class").HasAttribute("table", "XXX");
        //}

        //[Test]
        //public void DefaultLazyAlwaysFalseIsApplied()
        //{
        //    new MappingTester<ExampleClass>()
        //        .Conventions(x => x.Add(DefaultLazy.AlwaysFalse()))
        //        .ForMapping(m => { })
        //        .RootElement.HasAttribute("default-lazy", "false");
        //}

        //private class ExampleClassTableConvention : IClassConvention
        //{
        //    public bool Accept(IClassMap target)
        //    {
        //        return string.IsNullOrEmpty(target.TableName);
        //    }

        //    public void Apply(IClassMap target)
        //    {
        //        target.WithTable("XXX");
        //    }
        //}
    }
}