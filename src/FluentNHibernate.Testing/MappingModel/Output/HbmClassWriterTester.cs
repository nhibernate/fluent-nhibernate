using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.MappingModel.Identity;
using Rhino.Mocks;
using FluentNHibernate.MappingModel.Collections;
using StructureMap.AutoMocking;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmClassWriterTester
    {
        private RhinoAutoMocker<HbmClassWriter> _mocker;
        private HbmClassWriter _classWriter;

        [SetUp]
        public void SetUp()
        {
            _mocker = new RhinoAutoMocker<HbmClassWriter>();
            _classWriter = _mocker.ClassUnderTest;
        }

        [Test]
        public void Should_produce_valid_hbm()
        {
            var classMapping = new ClassMapping {Name = "class1", Id = new IdMapping()};

            _mocker.Get<IXmlWriter<IIdentityMapping>>()
                .Expect(x => x.Write(classMapping.Id)).Return(new HbmId { generator = new HbmGenerator { @class = "native" } });

            _classWriter.ShouldGenerateValidOutput(classMapping);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<ClassMapping>();
            testHelper.Check(x => x.Name, "class1").MapsToAttribute("name");
			testHelper.Check(x => x.Tablename, "table1").MapsToAttribute("table");

            testHelper.VerifyAll(_classWriter);
        }


        [Test]
        public void Should_write_the_id()
        {
            var classMapping = new ClassMapping { Id = new IdMapping() };
            _mocker.Get<IXmlWriter<IIdentityMapping>>()
                .Expect(x => x.Write(classMapping.Id))
                .Return(new HbmId());

            _classWriter.VerifyXml(classMapping)
                .Element("id").Exists();            
        }

        [Test]
        public void Should_write_the_properties()
        {            
            var classMapping = new ClassMapping();
            classMapping.AddProperty(new PropertyMapping());
            _mocker.Get<IXmlWriter<PropertyMapping>>()
                .Expect(x => x.Write(classMapping.Properties.First()))
                .Return(new HbmProperty());

            _classWriter.VerifyXml(classMapping)
                .Element("property").Exists();            
        }

        [Test]
        public void Should_write_the_collections()
        {
            var classMapping = new ClassMapping();
            classMapping.AddCollection(new BagMapping());

            _mocker.Get<IXmlWriter<ICollectionMapping>>()
                .Expect(x => x.Write(classMapping.Collections.First()))
                .Return(new HbmBag());
            
            _classWriter.VerifyXml(classMapping)
                .Element("bag").Exists();   
        }

        [Test]
        public void Should_write_the_references()
        {
            var classMapping = new ClassMapping();
            classMapping.AddReference(new ManyToOneMapping());

            _mocker.Get<IXmlWriter<ManyToOneMapping>>()
                .Expect(x => x.Write(classMapping.References.First()))
                .Return(new HbmManyToOne());
            
            _classWriter.VerifyXml(classMapping)
                .Element("many-to-one").Exists();   
        }

        [Test]
        public void Should_write_the_subclasses()
        {
            var classMapping = new ClassMapping();
            classMapping.AddSubclass(new JoinedSubclassMapping());

            _mocker.Get<IXmlWriter<ISubclassMapping>>()
                .Expect(x => x.Write(classMapping.Subclasses.First()))
                .Return(new HbmJoinedSubclass());

            _classWriter.VerifyXml(classMapping)
                .Element("joined-subclass").Exists();
        }

        [Test]
        public void Should_write_the_discriminator()
        {
            var classMapping = new ClassMapping {Discriminator = new DiscriminatorMapping()};

            _mocker.Get<IXmlWriter<DiscriminatorMapping>>()
                .Expect(x => x.Write(classMapping.Discriminator)).Return(new HbmDiscriminator());

            _classWriter.VerifyXml(classMapping)
                .Element("discriminator").Exists();
        }

        [Test]
        public void Should_write_the_components()
        {
            var classMapping = new ClassMapping();
            classMapping.AddComponent(new ComponentMapping());

            _mocker.Get<IXmlWriter<ComponentMapping>>()
                .Expect(x => x.Write(classMapping.Components.First())).Return(new HbmComponent());

            _classWriter.VerifyXml(classMapping)
                .Element("component").Exists();
        }

    }
}