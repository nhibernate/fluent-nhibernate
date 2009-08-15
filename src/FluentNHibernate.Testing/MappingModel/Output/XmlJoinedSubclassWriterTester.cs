using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlJoinedSubclassWriterTester
    {
        private IXmlWriter<JoinedSubclassMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<JoinedSubclassMapping>>();
        }

        [Test]
        public void ShouldWriteExtendsAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Extends, "ext").MapsToAttribute("extends");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTableAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.TableName, "tbl").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteProxyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Proxy, "p").MapsToAttribute("proxy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCheckAttribute()
        {
            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Check, "chk").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDynamicUpdateAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.DynamicUpdate, true).MapsToAttribute("dynamic-update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDynamicInsertAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.DynamicInsert, true).MapsToAttribute("dynamic-insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSelectBeforeUpdateAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.SelectBeforeUpdate, true).MapsToAttribute("select-before-update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteAbstractAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Abstract, true).MapsToAttribute("abstract");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSubselectAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Subselect, "subselect").MapsToAttribute("subselect");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePersisterAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Persister, new TypeReference(typeof(string))).MapsToAttribute("persister");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteBatchSizeAttribute()
        {

            var testHelper = new XmlWriterTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteProperties()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddProperty(new PropertyMapping());

            writer.VerifyXml(mapping)
                .Element("property").Exists();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddReference(new ManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void ShouldWriteOneToOnes()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddOneToOne(new OneToOneMapping());

            writer.VerifyXml(mapping)
                .Element("one-to-one").Exists();
        }

        [Test]
        public void ShouldWriteComponents()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddComponent(new ComponentMapping());

            writer.VerifyXml(mapping)
                .Element("component").Exists();
        }

        [Test]
        public void ShouldWriteDynamicComponents()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddComponent(new DynamicComponentMapping());

            writer.VerifyXml(mapping)
                .Element("dynamic-component").Exists();
        }

        [Test]
        public void ShouldWriteAny()
        {
            var mapping = new JoinedSubclassMapping();
            
            mapping.AddAny(new AnyMapping());

            writer.VerifyXml(mapping)
                .Element("any").Exists();
        }

        [Test]
        public void ShouldWriteMap()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddCollection(new MapMapping());

            writer.VerifyXml(mapping)
                .Element("map").Exists();
        }

        [Test]
        public void ShouldWriteSet()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddCollection(new SetMapping());

            writer.VerifyXml(mapping)
                .Element("set").Exists();
        }

        [Test]
        public void ShouldWriteList()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddCollection(new ListMapping());

            writer.VerifyXml(mapping)
                .Element("list").Exists();
        }

        [Test]
        public void ShouldWriteBag()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddCollection(new BagMapping());

            writer.VerifyXml(mapping)
                .Element("bag").Exists();
        }

        [Test, Ignore]
        public void ShouldWriteArray()
        {
            Assert.Fail();
        }

        [Test, Ignore]
        public void ShouldWritePrimitiveArray()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteSubclass()
        {
            var mapping = new JoinedSubclassMapping();

            mapping.AddSubclass(new JoinedSubclassMapping());

            writer.VerifyXml(mapping)
                .Element("joined-subclass").Exists();
        }
    }
}
