using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlJoinWriterTester
    {
        private IXmlWriter<JoinMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<JoinMapping>>();
        }

        [Test]
        public void ShouldWriteTableAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.TableName, "tbl").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFetchAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.Fetch, "fetch").MapsToAttribute("fetch");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInverseAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.Inverse, true).MapsToAttribute("inverse");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptionalAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.Optional, true).MapsToAttribute("optional");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCatalogAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.Catalog, "catalog").MapsToAttribute("catalog");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSubselectAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.Subselect, "subselect").MapsToAttribute("subselect");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteKey()
        {
            var mapping = new JoinMapping();

            mapping.Key = new KeyMapping();

            writer.VerifyXml(mapping)
                .Element("key").Exists();
        }

        [Test]
        public void ShouldWriteProperties()
        {
            var mapping = new JoinMapping();

            mapping.AddProperty(new PropertyMapping());

            writer.VerifyXml(mapping)
                .Element("property").Exists();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            var mapping = new JoinMapping();

            mapping.AddReference(new ManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void ShouldWriteComponents()
        {
            var mapping = new JoinMapping();

            mapping.AddComponent(new ComponentMapping());

            writer.VerifyXml(mapping)
                .Element("component").Exists();
        }

        [Test]
        public void ShouldWriteDynamicComponents()
        {
            var mapping = new JoinMapping();

            mapping.AddComponent(new DynamicComponentMapping());

            writer.VerifyXml(mapping)
                .Element("dynamic-component").Exists();
        }

        [Test]
        public void ShouldWriteAny()
        {
            var mapping = new JoinMapping();
            
            mapping.AddAny(new AnyMapping());

            writer.VerifyXml(mapping)
                .Element("any").Exists();
        }
    }
}
