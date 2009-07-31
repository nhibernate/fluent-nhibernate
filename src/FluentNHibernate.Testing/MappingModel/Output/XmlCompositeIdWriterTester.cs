using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlCompositeIdWriterTester
    {
        private IXmlWriter<CompositeIdMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<CompositeIdMapping>>();
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<CompositeIdMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<CompositeIdMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            var testHelper = new XmlWriterTestHelper<CompositeIdMapping>();
            testHelper.Check(x => x.Class, new TypeReference("class")).MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteMappedAttribute()
        {
            var testHelper = new XmlWriterTestHelper<CompositeIdMapping>();
            testHelper.Check(x => x.Mapped, true).MapsToAttribute("mapped");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUnsavedValueAttribute()
        {
            var testHelper = new XmlWriterTestHelper<CompositeIdMapping>();
            testHelper.Check(x => x.UnsavedValue, "u-value").MapsToAttribute("unsaved-value");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteKeyProperties()
        {
            var mapping = new CompositeIdMapping();

            mapping.AddKeyProperty(new KeyPropertyMapping());

            writer.VerifyXml(mapping)
                .Element("key-property").Exists();
        }

        [Test]
        public void ShouldWriteKeyManyToOnes()
        {
            var mapping = new CompositeIdMapping();

            mapping.AddKeyManyToOne(new KeyManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("key-many-to-one").Exists();
        }
    }
}