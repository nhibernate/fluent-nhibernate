using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlManyToOneWriterTester
    {
        private IXmlWriter<ManyToOneMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ManyToOneMapping>>();
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCascadeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Cascade, "all").MapsToAttribute("cascade");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Class, new TypeReference(typeof(Record))).MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFetchAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Fetch, "select").MapsToAttribute("fetch");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteForeignKeyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.ForeignKey, "fk").MapsToAttribute("foreign-key");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteProxyLazyAttribute()
        {
            var mapping = new ManyToOneMapping();

            mapping.Lazy = true;

            writer.VerifyXml(mapping)
                .HasAttribute("lazy", "proxy");
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Name, "nm").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNotFoundAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.NotFound, "nf").MapsToAttribute("not-found");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePropertyRefAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.PropertyRef, "pr").MapsToAttribute("property-ref");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUpdateAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Update, true).MapsToAttribute("update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new ManyToOneMapping();

            mapping.AddColumn(new ColumnMapping());

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }

    }
}