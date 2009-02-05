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

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmJoinedSubclassWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping { Name = "joinedsubclass1", Key = new KeyMapping() };
            var keyWriter = MockRepository.GenerateStub<IHbmWriter<KeyMapping>>();
            keyWriter.Expect(x => x.Write(joinedSubclassMapping.Key)).Return(new HbmKey());
            var writer = new HbmJoinedSubclassWriter(null, null, null, keyWriter);

            writer.ShouldGenerateValidOutput(joinedSubclassMapping);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Name, "mapping1").MapsToAttribute("name");

            var writer = new HbmJoinedSubclassWriter(null, null, null, null);
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void Should_write_the_key()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping { Key = new KeyMapping() };

            var keyWriter = MockRepository.GenerateStub<IHbmWriter<KeyMapping>>();
            keyWriter.Expect(x => x.Write(joinedSubclassMapping.Key)).Return(new HbmKey());
            var writer = new HbmJoinedSubclassWriter(null, null, null, keyWriter);

            writer.VerifyXml(joinedSubclassMapping)
                .Element("key").Exists();
        }

        [Test]
        public void Should_write_the_subclasses()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();
            joinedSubclassMapping.AddSubclass(new JoinedSubclassMapping());

            var writer = new HbmJoinedSubclassWriter(null, null, null, null);
            writer.VerifyXml(joinedSubclassMapping)
                .Element("joined-subclass").Exists();
        }

        [Test]
        public void Should_write_multiple_nestings_of_subclasses()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();

            joinedSubclassMapping.AddSubclass(new JoinedSubclassMapping { Name = "Child" });
            joinedSubclassMapping.Subclasses.First().AddSubclass(new JoinedSubclassMapping { Name = "Grandchild" });

            var writer = new HbmJoinedSubclassWriter(null, null, null, null);
            writer.VerifyXml(joinedSubclassMapping)
                .Element("joined-subclass").Exists().HasAttribute("name", "Child")
                .Element("joined-subclass").Exists().HasAttribute("name", "Grandchild");
        }

        [Test]
        public void Should_write_the_collections()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();
            joinedSubclassMapping.AddCollection(new BagMapping());

            var collectionWriter = MockRepository.GenerateStub<IHbmWriter<ICollectionMapping>>();
            collectionWriter.Expect(x => x.Write(joinedSubclassMapping.Collections.First())).Return(new HbmBag());

            var writer = new HbmJoinedSubclassWriter(collectionWriter, null, null, null);
            writer.VerifyXml(joinedSubclassMapping)
                .Element("bag").Exists();   
        }

        [Test]
        public void Should_write_the_properties()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();
            joinedSubclassMapping.AddProperty(new PropertyMapping());

            var propertyWriter = MockRepository.GenerateStub<IHbmWriter<PropertyMapping>>();
            propertyWriter.Expect(x => x.Write(joinedSubclassMapping.Properties.First())).Return(new HbmProperty());

            var writer = new HbmJoinedSubclassWriter(null, propertyWriter, null, null);
            writer.VerifyXml(joinedSubclassMapping)
                .Element("property").Exists();
        }

        [Test]
        public void Should_write_the_references()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();            
            joinedSubclassMapping.AddReference(new ManyToOneMapping());

            var referenceWriter = MockRepository.GenerateStub<IHbmWriter<ManyToOneMapping>>();
            referenceWriter.Expect(x => x.Write(joinedSubclassMapping.References.First())).Return(new HbmManyToOne());

            var writer = new HbmJoinedSubclassWriter(null, null, referenceWriter, null);
            writer.VerifyXml(joinedSubclassMapping)
                .Element("many-to-one").Exists();
        }


    }
}
