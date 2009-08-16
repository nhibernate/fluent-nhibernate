using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlColumnWriterTester
    {
        private XmlColumnWriter writer;

        [Test]
        public void ShouldWriteCheckAttribute()
        {
            writer = new XmlColumnWriter();
            var testHelper = new XmlWriterTestHelper<ColumnMapping>();
            testHelper.Check(x => x.Check, "ck").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteIndexAttribute()
        {
            writer = new XmlColumnWriter();
            var testHelper = new XmlWriterTestHelper<ColumnMapping>();
            testHelper.Check(x => x.Index, "ix").MapsToAttribute("index");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLengthAttribute()
        {
            writer = new XmlColumnWriter();
            var testHelper = new XmlWriterTestHelper<ColumnMapping>();
            testHelper.Check(x => x.Length, 10).MapsToAttribute("length");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlColumnWriter();
            var testHelper = new XmlWriterTestHelper<ColumnMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNotNullAttribute()
        {
            writer = new XmlColumnWriter();
            var testHelper = new XmlWriterTestHelper<ColumnMapping>();
            testHelper.Check(x => x.NotNull, true).MapsToAttribute("not-null");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSqlTypeAttribute()
        {
            writer = new XmlColumnWriter();
            var testHelper = new XmlWriterTestHelper<ColumnMapping>();
            testHelper.Check(x => x.SqlType, "type").MapsToAttribute("sql-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUniqueAttribute()
        {
            writer = new XmlColumnWriter();
            var testHelper = new XmlWriterTestHelper<ColumnMapping>();
            testHelper.Check(x => x.Unique, true).MapsToAttribute("unique");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUniqueKeyAttribute()
        {
            writer = new XmlColumnWriter();
            var testHelper = new XmlWriterTestHelper<ColumnMapping>();
            testHelper.Check(x => x.UniqueKey, "uk").MapsToAttribute("unique-key");

            testHelper.VerifyAll(writer);
        }
    }
}