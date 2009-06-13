using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlKeyManyToOneWriterTester
    {
        private IXmlWriter<KeyManyToOneMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<KeyManyToOneMapping>>();
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyManyToOneMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyManyToOneMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyManyToOneMapping>();
            testHelper.Check(x => x.Class, new TypeReference("type")).MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteForeignKeyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyManyToOneMapping>();
            testHelper.Check(x => x.ForeignKey, "fk").MapsToAttribute("foreign-key");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyManyToOneMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNotFoundAttribute()
        {
            var testHelper = new XmlWriterTestHelper<KeyManyToOneMapping>();
            testHelper.Check(x => x.NotFound, "nf").MapsToAttribute("not-found");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new KeyManyToOneMapping();

            mapping.AddColumn(new ColumnMapping());

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}