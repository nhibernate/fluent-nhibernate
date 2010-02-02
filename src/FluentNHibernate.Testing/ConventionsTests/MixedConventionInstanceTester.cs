using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class MixedConventionInstanceTester
    {
        [Test]
        public void CanApplySameInstanceToMultipleParts()
        {
            new MappingTester<ExampleClass>()
                .Conventions(conventions => conventions.Add(new CustomConvention()))
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.LineOne);
                })
                .Element("class/id/column").HasAttribute("name", "id-col")
                .Element("class/property[@name='LineOne']/column").HasAttribute("name", "prop-col");
        }

        private class CustomConvention : IIdConvention, IPropertyConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Column("id-col");
            }

            public void Apply(IPropertyInstance instance)
            {
                instance.Column("prop-col");
            }
        }
    }
}