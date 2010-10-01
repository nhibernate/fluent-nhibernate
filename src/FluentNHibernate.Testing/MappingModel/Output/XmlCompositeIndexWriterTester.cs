using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlCompositeIndexWriterTester
    {
        private IXmlWriter<CompositeIndexMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<CompositeIndexMapping>>();
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            var testHelper = new XmlWriterTestHelper<CompositeIndexMapping>();

            testHelper.Check(x => x.Type, new TypeReference("t")).MapsToAttribute("class");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteProperties()
        {
            var mapping = new CompositeIndexMapping();
            mapping.AddProperty(new KeyPropertyMapping());

            writer.VerifyXml(mapping)
                .Element("key-property").Exists();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            var mapping = new CompositeIndexMapping();
            mapping.AddReference(new KeyManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("key-many-to-one").Exists();
        }
    }
}