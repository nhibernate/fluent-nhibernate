using System;
using System.Collections;
using System.Collections.Generic;
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
                        m.Map(x => (string)x["Name"]);
                        m.Map(x => (int)x["Age"]);
                        m.Map(x => (string)x["Profession"]);
                    }))
                .Element("//class/dynamic-component/property[@name='Name']").Exists()
                .Element("//class/dynamic-component/property[@name='Age']").Exists()
                .Element("//class/dynamic-component/property[@name='Profession']").Exists();
        }

        [Test]
        public void DynamicComponentIsGeneratedWithOnlyOnePropertyReference()
        {
            //Regression test for issue 223
            new MappingTester<PropertyTarget>()
                .ForMapping(c =>
                    c.DynamicComponent(x => x.ExtensionData, m =>
                    {
                        m.Map(x => (string)x["Name"]);
                    }))
                .Element("//class/dynamic-component").HasThisManyChildNodes(1);
        }

        [Test]
        public void CanGenerateDynamicComponentsWithInt32Property()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c =>
                    c.DynamicComponent(x => x.ExtensionData, m =>
                    {
                        m.Map(x => (int)x["Age"]);
                    })).Element("//class/dynamic-component/property").HasAttribute("type", typeof(Int32).AssemblyQualifiedName);

        }

        [Test]
        public void CanGenerateDynamicComponentsWithStringProperty()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(c =>
                    c.DynamicComponent(x => x.ExtensionData, m =>
                    {
                        m.Map(x => (string)x["Name"]);
                    })).Element("//class/dynamic-component/property").HasAttribute("type", typeof(string).AssemblyQualifiedName);
        }

        [Test]
        public void CanMapReferences()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                        c.References(x => (PropertyReferenceTarget)x["Parent"])))
                .Element("class/dynamic-component/many-to-one").Exists();
        }

        [Test]
        public void CanMapHasOne()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                        c.HasOne(x => (PropertyReferenceTarget)x["Parent"])))
                .Element("class/dynamic-component/one-to-one").Exists();
        }

        [Test]
        public void CanMapHasMany()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                        c.HasMany(x => (IList<PropertyReferenceTarget>)x["Children"])))
                .Element("class/dynamic-component/bag").Exists();
        }

        [Test]
        public void CanMapHasManyToMany()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                        c.HasManyToMany(x => (IList<PropertyReferenceTarget>)x["Children"])))
                .Element("class/dynamic-component/bag").Exists();
        }

        [Test]
        public void CanMapComponent()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                        c.Component(x => (PropertyReferenceTarget)x["Component"], sc => { })))
                .Element("class/dynamic-component/component").Exists();
        }

        [Test]
        public void CanMapDynamicComponent()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                        c.DynamicComponent(x => (IDictionary)x["Component"], sc => { })))
                .Element("class/dynamic-component/dynamic-component").Exists();
        }

        [Test]
        public void DynamicComponentHasName()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c => { }))
                .Element("class/dynamic-component").HasAttribute("name", "ExtensionData");
        }

        [Test]
        public void CanSetParentRef()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                        c.ParentReference(x => x["Parent"])))
                .Element("class/dynamic-component/parent").HasAttribute("name", "Parent");
        }

        [Test]
        public void ComponentCanSetInsert()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                    {
                        c.Insert();
                    }))
                .Element("class/dynamic-component").HasAttribute("insert", "true");
        }

        [Test]
        public void ComponentCanSetUpdate()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                    m.DynamicComponent(x => x.ExtensionData, c =>
                    {
                        c.Update();
                    }))
                .Element("class/dynamic-component").HasAttribute("update", "true");
        }
    }
}
