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
    public class XmlDynamicComponentWriterTester
    {
        private IXmlWriter<IComponentMapping> writer;

        private XmlWriterTestHelper<IComponentMapping> create_helper()
        {
            var helper = new XmlWriterTestHelper<IComponentMapping>();
            helper.CreateInstance(() => new ComponentMapping(ComponentType.DynamicComponent));
            return helper;
        }

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<IComponentMapping>>();
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            var testHelper = create_helper();
            
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            var testHelper = create_helper();

            testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUpdateAttribute()
        {
            var testHelper = create_helper();

            testHelper.Check(x => x.Update, true).MapsToAttribute("update");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            var testHelper = create_helper();

            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteComponents()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddComponent(new ComponentMapping(ComponentType.Component));

            writer.VerifyXml(mapping)
                .Element("component").Exists();
        }

        [Test]
        public void ShouldWriteDynamicComponents()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddComponent(new ComponentMapping(ComponentType.DynamicComponent));

            writer.VerifyXml(mapping)
                .Element("dynamic-component").Exists();
        }

        [Test]
        public void ShouldWriteProperties()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddProperty(new PropertyMapping());

            writer.VerifyXml(mapping)
                .Element("property").Exists();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddReference(new ManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("many-to-one").Exists();
        }

        [Test]
        public void ShouldWriteOneToOnes()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddOneToOne(new OneToOneMapping());

            writer.VerifyXml(mapping)
                .Element("one-to-one").Exists();
        }

        [Test]
        public void ShouldWriteAnys()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddAny(new AnyMapping());

            writer.VerifyXml(mapping)
                .Element("any").Exists();
        }

        [Test]
        public void ShouldWriteMaps()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddCollection(CollectionMapping.Map());

            writer.VerifyXml(mapping)
                .Element("map").Exists();
        }

        [Test]
        public void ShouldWriteSets()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddCollection(CollectionMapping.Set());

            writer.VerifyXml(mapping)
                .Element("set").Exists();
        }

        [Test]
        public void ShouldWriteBags()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddCollection(CollectionMapping.Bag());

            writer.VerifyXml(mapping)
                .Element("bag").Exists();
        }

        [Test]
        public void ShouldWriteLists()
        {
            var mapping = new ComponentMapping(ComponentType.DynamicComponent);

            mapping.AddCollection(CollectionMapping.List());

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