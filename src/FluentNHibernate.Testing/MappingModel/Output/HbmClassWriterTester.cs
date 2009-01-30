using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.MappingModel.Identity;
using Rhino.Mocks;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmClassWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var classMapping = new ClassMapping {Name = "class1", Id = new IdMapping()};
            var identityWriter = MockRepository.GenerateStub<IHbmWriter<IIdentityMapping>>();
            identityWriter.Expect(x => x.Write(classMapping.Id)).Return(new HbmId { generator = new HbmGenerator { @class = "native"}});
            var writer = new HbmClassWriter(identityWriter, null, null, null, null);

            writer.ShouldGenerateValidOutput(classMapping);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<ClassMapping>();
            testHelper.Check(x => x.Name, "class1").MapsToAttribute("name");
			testHelper.Check(x => x.Tablename, "table1").MapsToAttribute("table");
        }


        [Test]
        public void Should_write_the_id()
        {
            var classMapping = new ClassMapping { Id = new IdMapping() };
            var idWriter = MockRepository.GenerateStub<IHbmWriter<IIdentityMapping>>();
            idWriter.Expect(x => x.Write(classMapping.Id)).Return(new HbmId());            
            
            var writer = new HbmClassWriter(idWriter, null, null, null, null);
            writer.VerifyXml(classMapping)
                .Element("id").Exists();            
        }

        [Test]
        public void Should_write_the_properties()
        {            
            var classMapping = new ClassMapping();
            classMapping.AddProperty(new PropertyMapping());

            var propertyWriter = MockRepository.GenerateStub<IHbmWriter<PropertyMapping>>();
            propertyWriter.Expect(x => x.Write(classMapping.Properties.First())).Return(new HbmProperty());
            
            var writer = new HbmClassWriter(null, null, propertyWriter, null, null);
            writer.VerifyXml(classMapping)
                .Element("property").Exists();            
        }

        [Test]
        public void Should_write_the_collections()
        {
            var classMapping = new ClassMapping();
            classMapping.AddCollection(new BagMapping());

            var collectionWriter = MockRepository.GenerateStub<IHbmWriter<ICollectionMapping>>();
            collectionWriter.Expect(x => x.Write(classMapping.Collections.First())).Return(new HbmBag());
            
            var writer = new HbmClassWriter(null, collectionWriter, null, null, null);
            writer.VerifyXml(classMapping)
                .Element("bag").Exists();   
        }

        [Test]
        public void Should_write_the_references()
        {
            var classMapping = new ClassMapping();
            classMapping.AddReference(new ManyToOneMapping());

            var referenceWriter = MockRepository.GenerateStub<IHbmWriter<ManyToOneMapping>>();
            referenceWriter.Expect(x => x.Write(classMapping.References.First())).Return(new HbmManyToOne());
            
            var writer = new HbmClassWriter(null, null, null, referenceWriter, null);
            writer.VerifyXml(classMapping)
                .Element("many-to-one").Exists();   
        }

        [Test]
        public void Should_write_the_subclasses()
        {
            var classMapping = new ClassMapping();
            classMapping.AddSubclass(new JoinedSubclassMapping());

            var subclassWriter = MockRepository.GenerateStub<IHbmWriter<ISubclassMapping>>();
            subclassWriter.Expect(x => x.Write(classMapping.Subclasses.First())).Return(new HbmJoinedSubclass());

            var writer = new HbmClassWriter(null, null, null, null, subclassWriter);
            writer.VerifyXml(classMapping)
                .Element("joined-subclass").Exists();
        }
    }
}