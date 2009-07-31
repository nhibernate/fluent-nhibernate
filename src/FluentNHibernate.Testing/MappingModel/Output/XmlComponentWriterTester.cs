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
    public class XmlComponentWriterTester
    {
        private IXmlWriter<ComponentMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ComponentMapping>>();
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ComponentMapping>();
            
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ComponentMapping>();

            testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ComponentMapping>();

            testHelper.Check(x => x.Class, new TypeReference("class")).MapsToAttribute("class");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUpdateAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ComponentMapping>();

            testHelper.Check(x => x.Update, true).MapsToAttribute("update");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ComponentMapping>();

            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ComponentMapping>();

            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ComponentMapping>();

            testHelper.Check(x => x.OptimisticLock, true).MapsToAttribute("optimistic-lock");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteComponents()
        {
            var mapping = new ComponentMapping();

            mapping.AddComponent(new ComponentMapping());

            writer.VerifyXml(mapping)
                .Element("component").Exists();
        }

        [Test]
        public void ShouldWriteDynamicComponents()
        {
            var mapping = new ComponentMapping();

            mapping.AddComponent(new DynamicComponentMapping());

            writer.VerifyXml(mapping)
                .Element("dynamic-component").Exists();
        }

        [Test]
        public void ShouldWriteProperties()
        {
            var mapping = new ComponentMapping();

            mapping.AddProperty(new PropertyMapping());

            writer.VerifyXml(mapping)
                .Element("property").Exists();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            var mapping = new ComponentMapping();

            mapping.AddReference(new ManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void ShouldWriteOneToOnes()
        {
            var mapping = new ComponentMapping();

            mapping.AddOneToOne(new OneToOneMapping());

            writer.VerifyXml(mapping)
                .Element("one-to-one").Exists();
        }

        [Test]
        public void ShouldWriteAnys()
        {
            var mapping = new ComponentMapping();

            mapping.AddAny(new AnyMapping());

            writer.VerifyXml(mapping)
                .Element("any").Exists();
        }

        [Test]
        public void ShouldWriteMaps()
        {
            var mapping = new ComponentMapping();

            mapping.AddCollection(new MapMapping());

            writer.VerifyXml(mapping)
                .Element("map").Exists();
        }

        [Test]
        public void ShouldWriteSets()
        {
            var mapping = new ComponentMapping();

            mapping.AddCollection(new SetMapping());

            writer.VerifyXml(mapping)
                .Element("set").Exists();
        }

        [Test]
        public void ShouldWriteBags()
        {
            var mapping = new ComponentMapping();

            mapping.AddCollection(new BagMapping());

            writer.VerifyXml(mapping)
                .Element("bag").Exists();
        }

        [Test]
        public void ShouldWriteLists()
        {
            var mapping = new ComponentMapping();

            mapping.AddCollection(new ListMapping());

            writer.VerifyXml(mapping)
                .Element("list").Exists();
        }

        [Test, Ignore]
        public void ShouldWriteArrays()
        {
            Assert.Fail();
        }

        [Test, Ignore]
        public void ShouldWritePrimitiveArrays()
        {
            Assert.Fail();
        }
    }
}