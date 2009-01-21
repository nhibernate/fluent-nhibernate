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
            var classMapping = new ClassMapping
                                   {
                                       Name = "class1",
                                       Id = new IdMapping()
                                   };
            var identityWriter = MockRepository.GenerateStub<IHbmWriter<IIdentityMapping>>();
            identityWriter.Expect(x => x.Write(classMapping.Id)).Return(new HbmId { generator = new HbmGenerator { @class = "native"}});
            var writer = new HbmClassWriter(identityWriter, null, null, null);

            writer.ShouldGenerateValidOutput(classMapping);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<ClassMapping, HbmClass>();
            testHelper.Check(x => x.Name, "class1").MapsTo(x => x.name);
        }

        [Test]
        public void Should_write_the_id()
        {
            var hbmId = new HbmId();
            var classMapping = new ClassMapping { Id = new IdMapping() };
            var idWriter = MockRepository.GenerateStub<IHbmWriter<IIdentityMapping>>();
            idWriter.Expect(x => x.Write(classMapping.Id)).Return(hbmId);            
            var writer = new HbmClassWriter(idWriter, null, null, null);

            var hbmClass = (HbmClass) writer.Write(classMapping);

            hbmClass.Id.ShouldEqual(hbmId);
        }

        [Test]
        public void Should_write_the_properties()
        {
            var hbmProperty1 = new HbmProperty();
            var hbmProperty2 = new HbmProperty();
            var classMapping = new ClassMapping();
            classMapping.AddProperty(new PropertyMapping());
            classMapping.AddProperty(new PropertyMapping());

            var propertyWriter = MockRepository.GenerateStub<IHbmWriter<PropertyMapping>>();
            propertyWriter.Expect(x => x.Write(classMapping.Properties.First())).Return(hbmProperty1);
            propertyWriter.Expect(x => x.Write(classMapping.Properties.Last())).Return(hbmProperty2);
            var writer = new HbmClassWriter(null, null, propertyWriter, null);

            var hbmClass = (HbmClass)writer.Write(classMapping);

            hbmClass.Items.ShouldHaveCount(2);
            hbmClass.Items.ShouldContain(hbmProperty1);
            hbmClass.Items.ShouldContain(hbmProperty2);            
        }

        [Test]
        public void Should_write_the_collections()
        {
            var hbmBag = new HbmBag();
            var hbmSet = new HbmSet();
            var classMapping = new ClassMapping();
            classMapping.AddCollection(new BagMapping());
            classMapping.AddCollection(new SetMapping());

            var collectionWriter = MockRepository.GenerateStub<IHbmWriter<ICollectionMapping>>();
            collectionWriter.Expect(x => x.Write(classMapping.Collections.First())).Return(hbmBag);
            collectionWriter.Expect(x => x.Write(classMapping.Collections.Last())).Return(hbmSet);
            var writer = new HbmClassWriter(null, collectionWriter, null, null);

            var hbmClass = (HbmClass)writer.Write(classMapping);

            hbmClass.Items.ShouldHaveCount(2);
            hbmClass.Items.ShouldContain(hbmBag);
            hbmClass.Items.ShouldContain(hbmSet);
        }

        [Test]
        public void Should_write_the_references()
        {
            var hbmManyToOne1 = new HbmManyToOne();
            var hbmManyToOne2 = new HbmManyToOne();
            
            var classMapping = new ClassMapping();
            classMapping.AddReference(new ManyToOneMapping());
            classMapping.AddReference(new ManyToOneMapping());

            var referenceWriter = MockRepository.GenerateStub<IHbmWriter<ManyToOneMapping>>();
            referenceWriter.Expect(x => x.Write(classMapping.References.First())).Return(hbmManyToOne1);
            referenceWriter.Expect(x => x.Write(classMapping.References.Last())).Return(hbmManyToOne2);
            var writer = new HbmClassWriter(null, null, null, referenceWriter);

            var hbmClass = (HbmClass)writer.Write(classMapping);

            hbmClass.Items.ShouldHaveCount(2);
            hbmClass.Items.ShouldContain(hbmManyToOne1);
            hbmClass.Items.ShouldContain(hbmManyToOne2);
        }
    }
}