using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlKeyPropertyWriterTester
    {
        private IXmlWriter<KeyPropertyMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<KeyPropertyMapping>>();
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyPropertyMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyPropertyMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyPropertyMapping>();
            testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new KeyPropertyMapping();

            mapping.AddColumn(new ColumnMapping());

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}