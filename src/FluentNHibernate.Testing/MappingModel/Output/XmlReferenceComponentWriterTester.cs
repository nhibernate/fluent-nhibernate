using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlReferenceComponentWriterTester
    {
        private IXmlWriter<ReferenceComponentMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ReferenceComponentMapping>>();
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = CreateTestHelper();

            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");
            testHelper.VerifyAll(writer);
        }

        private XmlWriterTestHelper<ReferenceComponentMapping> CreateTestHelper()
        {
            var testHelper = new XmlWriterTestHelper<ReferenceComponentMapping>();

            testHelper.CreateInstance(CreateInstance);
            return testHelper;
        }

        private ReferenceComponentMapping CreateInstance()
        {
            var property = new DummyPropertyInfo("ComponentProperty", typeof(ComponentTarget)).ToMember();
            var instance = new ReferenceComponentMapping(ComponentType.Component, property, typeof(ComponentTarget), typeof(Target), null);

            instance.AssociateExternalMapping(new ExternalComponentMapping(ComponentType.Component));

            return instance;
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = CreateTestHelper();

            testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            var testHelper = CreateTestHelper();

            testHelper.Check(x => x.Class, new TypeReference("class")).MapsToAttribute("class");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUpdateAttribute()
        {
            var testHelper = CreateTestHelper();

            testHelper.Check(x => x.Update, true).MapsToAttribute("update");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            var testHelper = CreateTestHelper();

            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            var testHelper = CreateTestHelper();

            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            var testHelper = CreateTestHelper();

            testHelper.Check(x => x.OptimisticLock, true).MapsToAttribute("optimistic-lock");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteComponents()
        {
            var mapping = CreateInstance();

            mapping.AddComponent(new ComponentMapping(ComponentType.Component));

            writer.VerifyXml(mapping)
                .Element("component").Exists();
        }

        [Test]
        public void ShouldWriteDynamicComponents()
        {
            var mapping = CreateInstance();

            mapping.AddComponent(new ComponentMapping(ComponentType.DynamicComponent));

            writer.VerifyXml(mapping)
                .Element("dynamic-component").Exists();
        }

        [Test]
        public void ShouldWriteProperties()
        {
            var mapping = CreateInstance();

            mapping.AddProperty(new PropertyMapping());

            writer.VerifyXml(mapping)
                .Element("property").Exists();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            var mapping = CreateInstance();

            mapping.AddReference(new ManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void ShouldWriteOneToOnes()
        {
            var mapping = CreateInstance();

            mapping.AddOneToOne(new OneToOneMapping());

            writer.VerifyXml(mapping)
                .Element("one-to-one").Exists();
        }

        [Test]
        public void ShouldWriteAnys()
        {
            var mapping = CreateInstance();

            mapping.AddAny(new AnyMapping());

            writer.VerifyXml(mapping)
                .Element("any").Exists();
        }

        [Test]
        public void ShouldWriteMaps()
        {
            var mapping = CreateInstance();

            mapping.AddCollection(new MapMapping());

            writer.VerifyXml(mapping)
                .Element("map").Exists();
        }

        [Test]
        public void ShouldWriteSets()
        {
            var mapping = CreateInstance();

            mapping.AddCollection(new SetMapping());

            writer.VerifyXml(mapping)
                .Element("set").Exists();
        }

        [Test]
        public void ShouldWriteBags()
        {
            var mapping = CreateInstance();

            mapping.AddCollection(new BagMapping());

            writer.VerifyXml(mapping)
                .Element("bag").Exists();
        }

        [Test]
        public void ShouldWriteLists()
        {
            var mapping = CreateInstance();

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

        private class Target {}
        private class ComponentTarget {}
    }
}