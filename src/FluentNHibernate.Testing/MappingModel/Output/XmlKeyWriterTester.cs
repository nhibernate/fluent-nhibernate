using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlKeyWriterTester
    {
        private IXmlWriter<KeyMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<KeyMapping>>();
        }

        [Test]
        public void ShouldWriteForeignKeyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyMapping>();
            testHelper.Check(x => x.ForeignKey, "fk").MapsToAttribute("foreign-key");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePropertyRefAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyMapping>();
            testHelper.Check(x => x.PropertyRef, "prop").MapsToAttribute("property-ref");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOnDeleteAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyMapping>();
            testHelper.Check(x => x.OnDelete, "cascade").MapsToAttribute("on-delete");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new KeyMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}