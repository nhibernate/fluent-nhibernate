using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class DynamicComponentTester
    {
        [Test]
        public void CanGenerateDynamicComponentsWithSingleProperties()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c =>
                    c.DynamicComponent(x => x.ExtensionData, m =>
                    {
                        m.Map(x => (string)x.ExtensionData["Name"]);
                        m.Map(x => (int)x.ExtensionData["Age"]);
                        m.Map(x => (string)x.ExtensionData["Profession"]);
                    }))
                .Element("//class/dynamic-component/property[@name='Name']").Exists()
                .Element("//class/dynamic-component/property[@name='Age']").Exists()
                .Element("//class/dynamic-component/property[@name='Profession']").Exists();

        }

        [Test]
        public void CanGenerateDynamicComponentsWithInt32Property()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c =>
                    c.DynamicComponent(x => x.ExtensionData, m =>
                    {
                        m.Map(x => (int)x.ExtensionData["Age"]);
                    })).Element("//class/dynamic-component/property").HasAttribute("type","Int32");

        }

        [Test]
        public void CanGenerateDynamicComponentsWithStringProperty()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c =>
                    c.DynamicComponent(x => x.ExtensionData, m =>
                    {
                        m.Map(x => (string)x.ExtensionData["Name"]);
                    })).Element("//class/dynamic-component/property").HasAttribute("type", "String");

        }
    }
}
