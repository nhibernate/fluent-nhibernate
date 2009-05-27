using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlMapWriterTester
    {
        private XmlMapWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteBatchSizeAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCascadeAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Cascade, "all").MapsToAttribute("cascade");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCheckAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Check, "ck").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCollectionTypeAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.CollectionType, "type").MapsToAttribute("collection-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFetchAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Fetch, "fetch").MapsToAttribute("fetch");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteGenericAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Generic, true).MapsToAttribute("generic");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInverseAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Inverse, true).MapsToAttribute("inverse");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.OptimisticLock, "lock").MapsToAttribute("optimistic-lock");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOrderByAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.OrderBy, "ord").MapsToAttribute("order-by");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOuterJoinAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.OuterJoin, "oj").MapsToAttribute("outer-join");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePersisterAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Persister, "p").MapsToAttribute("persister");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTableAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.TableName, "table").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteWhereAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Where, "x = 1").MapsToAttribute("where");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSortAttribute()
        {
            writer = new XmlMapWriter(null, null, null);
            var testHelper = new XmlWriterTestHelper<MapMapping>();
            testHelper.Check(x => x.Sort, "asc").MapsToAttribute("sort");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteKey()
        {
            var mapping = new MapMapping
            {
                Key = new KeyMapping()
            };

            writer = new XmlMapWriter(new XmlKeyWriter(null), null, null);
            writer.VerifyXml(mapping)
                .Element("key").Exists();
        }

        [Test]
        public void ShouldWriteRelationshipElement()
        {
            var mapping = new MapMapping();

            mapping.Relationship = new OneToManyMapping();

            writer = new XmlMapWriter(null, new XmlCollectionRelationshipWriter(new XmlOneToManyWriter(), null), null);
            writer.VerifyXml(mapping)
                .Element("one-to-many").Exists();
        }

        [Test]
        public void ShouldWriteCacheElement()
        {
            var mapping = new MapMapping();

            mapping.Cache = new CacheMapping();

            writer = new XmlMapWriter(null, null, new XmlCacheWriter());
            writer.VerifyXml(mapping)
                .Element("cache").Exists();
        }
    }
}