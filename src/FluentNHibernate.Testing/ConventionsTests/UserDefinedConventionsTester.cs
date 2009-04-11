using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.FluentInterface;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class UserDefinedConventionsTester
    {
        [Test]
        public void ShouldApplyUserConventionsBeforeDefaults()
        {
            new MappingTester<ExampleClass>()
                .Conventions(x => x.Add<ExampleClassTableConvention>())
                .ForMapping(m => { })
                .Element("class").HasAttribute("table", "XXX");
        }

        private class ExampleClassTableConvention : IClassConvention
        {
            public bool Accept(IClassMap target)
            {
                return string.IsNullOrEmpty(target.TableName);
            }

            public void Apply(IClassMap target)
            {
                target.WithTable("XXX");
            }
        }
    }
}