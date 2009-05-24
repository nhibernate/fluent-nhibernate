using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlSetWriterTester
    {
        private XmlSetWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteBatchSizeAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCascadeAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Cascade, "all").MapsToAttribute("cascade");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCheckAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Check, "ck").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCollectionTypeAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.CollectionType, "type").MapsToAttribute("collection-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFetchAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Fetch, "fetch").MapsToAttribute("fetch");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteGenericAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Generic, true).MapsToAttribute("generic");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInverseAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Inverse, true).MapsToAttribute("inverse");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.OptimisticLock, "lock").MapsToAttribute("optimistic-lock");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOrderByAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.OrderBy, "ord").MapsToAttribute("order-by");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOuterJoinAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.OuterJoin, "oj").MapsToAttribute("outer-join");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePersisterAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Persister, "p").MapsToAttribute("persister");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTableAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.TableName, "table").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteWhereAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Where, "x = 1").MapsToAttribute("where");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSortAttribute()
        {
            writer = new XmlSetWriter(null, null);
            var testHelper = new XmlWriterTestHelper<SetMapping>();
            testHelper.Check(x => x.Sort, "asc").MapsToAttribute("sort");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteKey()
        {
            var mapping = new SetMapping
            {
                Key = new KeyMapping()
            };

            writer = new XmlSetWriter(new XmlKeyWriter(null), null);
            writer.VerifyXml(mapping)
                .Element("key").Exists();
        }

        [Test]
        public void ShouldWriteRelationshipElement()
        {
            var mapping = new SetMapping();

            mapping.Relationship = new OneToManyMapping();

            writer = new XmlSetWriter(null, new XmlCollectionRelationshipWriter(new XmlOneToManyWriter(), null));
            writer.VerifyXml(mapping)
                .Element("one-to-many").Exists();
        }
    }
}