using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmComponentWriterTester
    {
        private RhinoAutoMocker<HbmComponentWriter> _mocker;
        private HbmComponentWriter _componentWriter;

        [SetUp]
        public void SetUp()
        {
            _mocker = new RhinoAutoMocker<HbmComponentWriter>();
            _componentWriter = _mocker.ClassUnderTest;
        }

        [Test]
        public void Should_produce_valid_hbm()
        {
            var component = new ComponentMapping {PropertyName = "EmailAddress"};
            
            _componentWriter.ShouldGenerateValidOutput(component);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<ComponentMapping>();
            testHelper.Check(x => x.PropertyName, "EmailAddress").MapsToAttribute("name");
            testHelper.Check(x => x.ClassName, "component1").MapsToAttribute("class");
            
            testHelper.VerifyAll(_componentWriter);
        }

        [Test]
        public void Should_write_the_properties()
        {
            var componentMapping = new ComponentMapping();
            componentMapping.AddProperty(new PropertyMapping());
            _mocker.Get<IXmlWriter<PropertyMapping>>()
                .Expect(x => x.Write(componentMapping.Properties.First()))
                .Return(new HbmProperty());

            _componentWriter.VerifyXml(componentMapping)
                .Element("property").Exists();
        }

        [Test]
        public void Should_write_the_collections()
        {
            var componentMapping = new ComponentMapping();            
            componentMapping.AddCollection(new BagMapping());

            _mocker.Get<IXmlWriter<ICollectionMapping>>()
                .Expect(x => x.Write(componentMapping.Collections.First()))
                .Return(new HbmBag());

            _componentWriter.VerifyXml(componentMapping)
                .Element("bag").Exists();
        }

        [Test]
        public void Should_write_the_references()
        {
            var componentMapping = new ComponentMapping();
            componentMapping.AddReference(new ManyToOneMapping());

            _mocker.Get<IXmlWriter<ManyToOneMapping>>()
                .Expect(x => x.Write(componentMapping.References.First()))
                .Return(new HbmManyToOne());

            _componentWriter.VerifyXml(componentMapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void Should_write_the_components()
        {
            var componentMapping = new ComponentMapping();
            componentMapping.AddComponent(new ComponentMapping());

            _componentWriter.VerifyXml(componentMapping)
                .Element("component").Exists();
        }

        [Test]
        public void Should_write_multiple_nestings_of_components()
        {
            var componentMapping = new ComponentMapping();

            componentMapping.AddComponent(new ComponentMapping { PropertyName = "Child"});
            componentMapping.Components.First().AddComponent(new ComponentMapping { PropertyName = "Grandchild" });

            _componentWriter.VerifyXml(componentMapping)
                .Element("component").Exists().HasAttribute("name", "Child")
                .Element("component").Exists().HasAttribute("name", "Grandchild");
        }

       

    }
}
