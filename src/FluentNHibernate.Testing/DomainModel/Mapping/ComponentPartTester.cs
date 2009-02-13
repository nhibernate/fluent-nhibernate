using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ComponentPartTester
    {
        [Test]
        public void ComponentCanIncludeParentReference()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.Component(x => x.Component, c =>
                    {
                        c.Map(x => x.Name);
                        c.WithParentReference(x => x.MyParent);
                    }))
                .Element("class/component/parent").Exists()
                .HasAttribute("name", "MyParent");
        }

        [Test]
        public void ComponentDoesntHaveUniqueAttributeByDefault()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.Component(x => x.Component, c =>
                    {
                        c.Map(x => x.Name);
                        c.WithParentReference(x => x.MyParent);
                    }))
                .Element("class/component").DoesntHaveAttribute("unique");
        }
    }
}
