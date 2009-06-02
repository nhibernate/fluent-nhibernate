using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlClassWriterTester
    {
        private IXmlWriter<ClassMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ClassMapping>>();
        }

        [Test]
        public void ShouldWriteTableAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.TableName, "tbl").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDiscriminatorValueAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.DiscriminatorValue, "val").MapsToAttribute("discriminator-value");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteMutableAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Mutable, true).MapsToAttribute("mutable");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePolymorphismAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Polymorphism, "poly").MapsToAttribute("polymorphism");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePersisterAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Persister, "p").MapsToAttribute("persister");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteWhereAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Where, "x = 1").MapsToAttribute("where");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteBatchSizeAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.OptimisticLock, "lock").MapsToAttribute("optimistic-lock");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCheckAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Check, "chk").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteProxyAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Proxy, "p").MapsToAttribute("proxy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDynamicUpdateAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.DynamicUpdate, true).MapsToAttribute("dynamic-update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDynamicInsertAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.DynamicInsert, true).MapsToAttribute("dynamic-insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSelectBeforeUpdateAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.SelectBeforeUpdate, true).MapsToAttribute("select-before-update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteAbstractAttribute()
        {

            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Abstract, true).MapsToAttribute("abstract");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCache()
        {
            var mapping = new ClassMapping();

            mapping.Cache = new CacheMapping();

            writer.VerifyXml(mapping)
                .Element("cache").Exists();
        }

        [Test]
        public void ShouldWriteId()
        {
            var mapping = new ClassMapping();

            mapping.Id = new IdMapping();

            writer.VerifyXml(mapping)
                .Element("id").Exists();
        }

        [Test]
        public void ShouldWriteCompositeId()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteVersion()
        {
            var mapping = new ClassMapping();

            mapping.Version = new VersionMapping();

            writer.VerifyXml(mapping)
                .Element("version").Exists();
        }

        [Test]
        public void ShouldWriteProperties()
        {
            var mapping = new ClassMapping();

            mapping.AddProperty(new PropertyMapping());

            writer.VerifyXml(mapping)
                .Element("property").Exists();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            var mapping = new ClassMapping();

            mapping.AddReference(new ManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void ShouldWriteOneToOnes()
        {
            var mapping = new ClassMapping();

            mapping.AddOneToOne(new OneToOneMapping());

            writer.VerifyXml(mapping)
                .Element("one-to-one").Exists();
        }

        [Test]
        public void ShouldWriteComponents()
        {
            var mapping = new ClassMapping();

            mapping.AddComponent(new ComponentMapping());

            writer.VerifyXml(mapping)
                .Element("component").Exists();
        }

        [Test]
        public void ShouldWriteDynamicComponents()
        {
            var mapping = new ClassMapping();

            mapping.AddComponent(new DynamicComponentMapping());

            writer.VerifyXml(mapping)
                .Element("dynamic-component").Exists();
        }

        [Test]
        public void ShouldWriteAny()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteMap()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteSet()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteList()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteBag()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteIdBag()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteArray()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWritePrimitiveArray()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteJoin()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteSubclass()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteJoinedSubclass()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteUnionSubclass()
        {
            Assert.Fail();
        }
    }
}
