using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlPropertyWriterTester
    {
        private XmlPropertyWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlPropertyWriter(null);
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlPropertyWriter(null);
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            writer = new XmlPropertyWriter(null);
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Type, "type").MapsToAttribute("type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFormulaAttribute()
        {
            writer = new XmlPropertyWriter(null);
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Formula, "form").MapsToAttribute("formula");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteGeneratedAttribute()
        {
            writer = new XmlPropertyWriter(null);
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Generated, "gen").MapsToAttribute("generated");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            writer = new XmlPropertyWriter(null);
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            writer = new XmlPropertyWriter(null);
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.OptimisticLock, "lock").MapsToAttribute("optimistic-lock");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUpdateAttribute()
        {
            writer = new XmlPropertyWriter(null);
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Update, true).MapsToAttribute("update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new PropertyMapping();

            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            writer = new XmlPropertyWriter(new XmlColumnWriter());
            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}