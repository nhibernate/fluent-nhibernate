using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlKeyWriterTester
    {
        private XmlKeyWriter writer;

        [Test]
        public void ShouldWriteForeignKeyAttribute()
        {
            writer = new XmlKeyWriter(null);
            var testHelper = new XmlWriterTestHelper<KeyMapping>();
            testHelper.Check(x => x.ForeignKey, "fk").MapsToAttribute("foreign-key");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePropertyRefAttribute()
        {
            writer = new XmlKeyWriter(null);
            var testHelper = new XmlWriterTestHelper<KeyMapping>();
            testHelper.Check(x => x.PropertyRef, "prop").MapsToAttribute("property-ref");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOnDeleteAttribute()
        {
            writer = new XmlKeyWriter(null);
            var testHelper = new XmlWriterTestHelper<KeyMapping>();
            testHelper.Check(x => x.OnDelete, "cascade").MapsToAttribute("on-delete");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new KeyMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            writer = new XmlKeyWriter(new XmlColumnWriter());
            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}