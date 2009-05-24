using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlIdWriterTester
    {
        private XmlIdWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlIdWriter(null, null);
            var testHelper = new XmlWriterTestHelper<IdMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlIdWriter(null, null);
            var testHelper = new XmlWriterTestHelper<IdMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            writer = new XmlIdWriter(null, null);
            var testHelper = new XmlWriterTestHelper<IdMapping>();
            testHelper.Check(x => x.Type, "type").MapsToAttribute("type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUnsavedValueAttribute()
        {
            writer = new XmlIdWriter(null, null);
            var testHelper = new XmlWriterTestHelper<IdMapping>();
            testHelper.Check(x => x.UnsavedValue, "u-value").MapsToAttribute("unsaved-value");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTheGenerator()
        {
            var mapping = new IdMapping
            {
                Generator = new GeneratorMapping { Class = "Class" }
            };

            writer = new XmlIdWriter(new XmlGeneratorWriter(), null);
            writer.VerifyXml(mapping)
                .Element("generator").Exists();
        }

        [Test]
        public void ShouldWriteTheColumns()
        {
            var mapping = new IdMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            writer = new XmlIdWriter(null, new XmlColumnWriter());
            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}