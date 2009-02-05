using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmSubclassWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var subclassMapping = new SubclassMapping { Name = "joinedsubclass1" };
            var writer = new HbmSubclassWriter(null, null, null);

            writer.ShouldGenerateValidOutput(subclassMapping);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<SubclassMapping>();
            testHelper.Check(x => x.Name, "mapping1").MapsToAttribute("name");
            testHelper.Check(x => x.DiscriminatorValue, "SalaryEmployee").MapsToAttribute("discriminator-value");

            var writer = new HbmSubclassWriter(null, null, null);
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void Should_write_the_subclasses()
        {
            var subclassMapping = new SubclassMapping();
            subclassMapping.AddSubclass(new SubclassMapping());

            var writer = new HbmSubclassWriter(null, null, null);
            writer.VerifyXml(subclassMapping)
                .Element("subclass").Exists();
        }

        [Test]
        public void Should_write_multiple_nestings_of_subclasses()
        {
            var subclassMapping = new SubclassMapping();

            subclassMapping.AddSubclass(new SubclassMapping { Name = "Child" });
            subclassMapping.Subclasses.First().AddSubclass(new SubclassMapping { Name = "Grandchild" });

            var writer = new HbmSubclassWriter(null, null, null);
            writer.VerifyXml(subclassMapping)
                .Element("subclass").Exists().HasAttribute("name", "Child")
                .Element("subclass").Exists().HasAttribute("name", "Grandchild");
        }

        [Test]
        public void Should_write_the_collections()
        {
            var subclassMapping = new SubclassMapping();
            subclassMapping.AddCollection(new BagMapping());

            var collectionWriter = MockRepository.GenerateStub<IHbmWriter<ICollectionMapping>>();
            collectionWriter.Expect(x => x.Write(subclassMapping.Collections.First())).Return(new HbmBag());

            var writer = new HbmSubclassWriter(collectionWriter, null, null);
            writer.VerifyXml(subclassMapping)
                .Element("bag").Exists();
        }

        [Test]
        public void Should_write_the_properties()
        {
            var subclassMapping = new SubclassMapping();
            subclassMapping.AddProperty(new PropertyMapping());

            var propertyWriter = MockRepository.GenerateStub<IHbmWriter<PropertyMapping>>();
            propertyWriter.Expect(x => x.Write(subclassMapping.Properties.First())).Return(new HbmProperty());

            var writer = new HbmSubclassWriter(null, propertyWriter, null);
            writer.VerifyXml(subclassMapping)
                .Element("property").Exists();
        }

        [Test]
        public void Should_write_the_references()
        {
            var subclassMapping = new SubclassMapping();
            subclassMapping.AddReference(new ManyToOneMapping());

            var referenceWriter = MockRepository.GenerateStub<IHbmWriter<ManyToOneMapping>>();
            referenceWriter.Expect(x => x.Write(subclassMapping.References.First())).Return(new HbmManyToOne());

            var writer = new HbmSubclassWriter(null, null, referenceWriter);
            writer.VerifyXml(subclassMapping)
                .Element("many-to-one").Exists();
        }
    }
}