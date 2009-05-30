using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlAnyWriterTester
    {
        private XmlAnyWriter writer;

        [Test]
        public void ShouldWriteIdTypeAttribute()
        {
            writer = new XmlAnyWriter(null);
            var testHelper = new XmlWriterTestHelper<AnyMapping>();
            testHelper.Check(x => x.IdType, "id").MapsToAttribute("id-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteMetaTypeAttribute()
        {
            writer = new XmlAnyWriter(null);
            var testHelper = new XmlWriterTestHelper<AnyMapping>();
            testHelper.Check(x => x.MetaType, "meta").MapsToAttribute("meta-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlAnyWriter(null);
            var testHelper = new XmlWriterTestHelper<AnyMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlAnyWriter(null);
            var testHelper = new XmlWriterTestHelper<AnyMapping>();
            testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            writer = new XmlAnyWriter(null);
            var testHelper = new XmlWriterTestHelper<AnyMapping>();
            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUpdateAttribute()
        {
            writer = new XmlAnyWriter(null);
            var testHelper = new XmlWriterTestHelper<AnyMapping>();
            testHelper.Check(x => x.Update, true).MapsToAttribute("update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCascadeAttribute()
        {
            writer = new XmlAnyWriter(null);
            var testHelper = new XmlWriterTestHelper<AnyMapping>();
            testHelper.Check(x => x.Cascade, "all").MapsToAttribute("cascade");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new AnyMapping();

            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            new XmlAnyWriter(new XmlColumnWriter())
                .VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}