using System.Linq;
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
    public class HbmSubclassWriterTester
    {
        private RhinoAutoMocker<HbmSubclassWriter> _mocker;
        private HbmSubclassWriter _subclassWriter;

        [SetUp]
        public void SetUp()
        {
            _mocker = new RhinoAutoMocker<HbmSubclassWriter>();
            _subclassWriter = _mocker.ClassUnderTest;
        }

        [Test]
        public void Should_produce_valid_hbm()
        {
            var subclassMapping = new SubclassMapping { Name = "joinedsubclass1" };

            _subclassWriter.ShouldGenerateValidOutput(subclassMapping);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<SubclassMapping>();
            testHelper.Check(x => x.Name, "mapping1").MapsToAttribute("name");
            testHelper.Check(x => x.DiscriminatorValue, "SalaryEmployee").MapsToAttribute("discriminator-value");

            testHelper.VerifyAll(_subclassWriter);
        }

        [Test]
        public void Should_write_the_subclasses()
        {
            var subclassMapping = new SubclassMapping();
            subclassMapping.AddSubclass(new SubclassMapping());

            _subclassWriter.VerifyXml(subclassMapping)
                .Element("subclass").Exists();
        }

        [Test]
        public void Should_write_multiple_nestings_of_subclasses()
        {
            var subclassMapping = new SubclassMapping();

            subclassMapping.AddSubclass(new SubclassMapping { Name = "Child" });
            subclassMapping.Subclasses.First().AddSubclass(new SubclassMapping { Name = "Grandchild" });

            _subclassWriter.VerifyXml(subclassMapping)
                .Element("subclass").Exists().HasAttribute("name", "Child")
                .Element("subclass").Exists().HasAttribute("name", "Grandchild");
        }

        [Test]
        public void Should_write_the_collections()
        {
            var subclassMapping = new SubclassMapping();
            subclassMapping.AddCollection(new BagMapping());

            _mocker.Get<IHbmWriter<ICollectionMapping>>()
                .Expect(x => x.Write(subclassMapping.Collections.First())).Return(new HbmBag());

            _subclassWriter.VerifyXml(subclassMapping)
                .Element("bag").Exists();
        }

        [Test]
        public void Should_write_the_properties()
        {
            var subclassMapping = new SubclassMapping();
            subclassMapping.AddProperty(new PropertyMapping());

            _mocker.Get<IHbmWriter<PropertyMapping>>()
                .Expect(x => x.Write(subclassMapping.Properties.First())).Return(new HbmProperty());

            _subclassWriter.VerifyXml(subclassMapping)
                .Element("property").Exists();
        }

        [Test]
        public void Should_write_the_references()
        {
            var subclassMapping = new SubclassMapping();
            subclassMapping.AddReference(new ManyToOneMapping());

            _mocker.Get<IHbmWriter<ManyToOneMapping>>()
                .Expect(x => x.Write(subclassMapping.References.First())).Return(new HbmManyToOne());

            _subclassWriter.VerifyXml(subclassMapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void Should_write_the_components()
        {
            var classMapping = new SubclassMapping();
            classMapping.AddComponent(new ComponentMapping());

            _mocker.Get<IHbmWriter<ComponentMapping>>()
                .Expect(x => x.Write(classMapping.Components.First())).Return(new HbmComponent());

            _subclassWriter.VerifyXml(classMapping)
                .Element("component").Exists();
        }
    }
}