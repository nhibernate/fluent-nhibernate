using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlIndexManyToManyWriterTester
    {
        private IXmlWriter<IndexMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<IndexMapping>>();
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<IndexMapping>();

            testHelper.CreateInstance(() => new IndexMapping { IsManyToMany = true });
            testHelper.Check(x => x.Type, new TypeReference("cls")).MapsToAttribute("class");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteForeignKey()
        {
            var mapping = new IndexMapping { IsManyToMany = true };

            mapping.ForeignKey = "FKTest";

            writer.VerifyXml(mapping)
                .HasAttribute("foreign-key","FKTest");
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new IndexMapping { IsManyToMany = true };

            mapping.AddColumn(new ColumnMapping());

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }

        [Test]
        public void ShouldWriteEntityName()
        {
            var testHelper = new XmlWriterTestHelper<IndexMapping>();
            testHelper.CreateInstance(() => new IndexMapping { IsManyToMany = true });
            testHelper.Check(x => x.EntityName, "name1").MapsToAttribute("entity-name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteManyToManyElement()
        {
            var mapping = new IndexMapping { IsManyToMany = true };

            writer.VerifyXml(mapping)
                .RootElement.HasName("index-many-to-many");
        }
    }
}