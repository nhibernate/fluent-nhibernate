using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlBagWriterTester
    {
        private XmlBagWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteBatchSizeAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCascadeAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Cascade, "all").MapsToAttribute("cascade");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCheckAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Check, "ck").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCollectionTypeAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.CollectionType, "type").MapsToAttribute("collection-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFetchAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Fetch, "fetch").MapsToAttribute("fetch");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteGenericAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Generic, true).MapsToAttribute("generic");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInverseAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Inverse, true).MapsToAttribute("inverse");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.OptimisticLock, "lock").MapsToAttribute("optimistic-lock");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOrderByAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.OrderBy, "ord").MapsToAttribute("order-by");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOuterJoinAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.OuterJoin, "oj").MapsToAttribute("outer-join");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePersisterAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Persister, "p").MapsToAttribute("persister");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTableAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.TableName, "table").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteWhereAttribute()
        {
            writer = new XmlBagWriter(null);
            var testHelper = new XmlWriterTestHelper<BagMapping>();
            testHelper.Check(x => x.Where, "x = 1").MapsToAttribute("where");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteKey()
        {
            var mapping = new BagMapping
            {
                Key = new KeyMapping()
            };

            writer = new XmlBagWriter(new XmlKeyWriter(null));
            writer.VerifyXml(mapping)
                .Element("key").Exists();
        }
    }
}