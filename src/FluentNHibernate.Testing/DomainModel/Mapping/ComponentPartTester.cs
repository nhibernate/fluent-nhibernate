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
                .ForMapping(c =>
                    c.Component<ComponentTarget>(x => x.Component, m => m.Map(x => x.Name))
                      .WithParentReference(y => y.MyParent)
                )
                .Element("class/component/parent").Exists()
                .HasAttribute("name", "MyParent");
                
        }
    }
}
