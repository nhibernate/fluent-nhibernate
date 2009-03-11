using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions;
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
                .Element("class/id").HasAttribute("applied", "true")
                .Element("class/property[@name='LineOne']").HasAttribute("applied", "true");
        }

        private class CustomConvention : IIdConvention, IPropertyConvention
        {
            public bool Accept(IIdentityPart target)
            {
                return true;
            }

            public void Apply(IIdentityPart target)
            {
                target.SetAttribute("applied", "true");
            }

            public bool Accept(IProperty target)
            {
                return true;
            }

            public void Apply(IProperty target)
            {
                target.SetAttribute("applied", "true");
            }
        }
    }
}