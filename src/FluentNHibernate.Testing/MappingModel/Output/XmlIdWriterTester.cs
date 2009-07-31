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
        private IXmlWriter<IdMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<IdMapping>>();
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<IdMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<IdMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<IdMapping>();
            testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLengthAttribute()
        {
            var testHelper = new XmlWriterTestHelper<IdMapping>();
            testHelper.Check(x => x.Length, 8).MapsToAttribute("length");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUnsavedValueAttribute()
        {
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

            writer.VerifyXml(mapping)
                .Element("generator").Exists();
        }

        [Test]
        public void ShouldWriteTheColumns()
        {
            var mapping = new IdMapping();
            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}