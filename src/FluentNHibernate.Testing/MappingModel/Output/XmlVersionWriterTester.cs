using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlVersionWriterTester
    {
        private IXmlWriter<VersionMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<VersionMapping>>();
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteGeneratedAttribute()
        {
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Generated, "always").MapsToAttribute("generated");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUnsavedValueAttribute()
        {
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.UnsavedValue, "u-value").MapsToAttribute("unsaved-value");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new VersionMapping();

            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}