using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlPropertyWriterTester
    {
        private IXmlWriter<PropertyMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<PropertyMapping>>();
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFormulaAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Formula, "form").MapsToAttribute("formula");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteGeneratedAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Generated, "gen").MapsToAttribute("generated");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.OptimisticLock, true).MapsToAttribute("optimistic-lock");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUpdateAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Update, true).MapsToAttribute("update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteIndexAttribute()
        {
            var testHelper = new XmlWriterTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Index, "index").MapsToAttribute("index");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new PropertyMapping();

            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}