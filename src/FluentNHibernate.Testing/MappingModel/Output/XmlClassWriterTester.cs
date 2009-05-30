using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlClassWriterTester
    {
        private XmlClassWriter writer;

        [Test]
        public void ShouldWriteTableAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.TableName, "tbl").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDiscriminatorValueAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.DiscriminatorValue, "val").MapsToAttribute("discriminator-value");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteMutableAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Mutable, true).MapsToAttribute("mutable");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePolymorphismAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Polymorphism, "poly").MapsToAttribute("polymorphism");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePersisterAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Persister, "p").MapsToAttribute("persister");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteWhereAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Where, "x = 1").MapsToAttribute("where");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteBatchSizeAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.OptimisticLock, "lock").MapsToAttribute("optimistic-lock");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCheckAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Check, "chk").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteProxyAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Proxy, "p").MapsToAttribute("proxy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDynamicUpdateAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.DynamicUpdate, true).MapsToAttribute("dynamic-update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDynamicInsertAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.DynamicInsert, true).MapsToAttribute("dynamic-insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSelectBeforeUpdateAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.SelectBeforeUpdate, true).MapsToAttribute("select-before-update");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteAbstractAttribute()
        {
            writer = new XmlClassWriter(null, null, null, null, null, null, null, null, null);
            var testHelper = new XmlWriterTestHelper<ClassMapping>();
            testHelper.Check(x => x.Abstract, true).MapsToAttribute("abstract");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCache()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteId()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteCompositeId()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteVersion()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteProperties()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteOneToOnes()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteComponents()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldWriteDynamicComponents()
        {
            Assert.Fail();
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
