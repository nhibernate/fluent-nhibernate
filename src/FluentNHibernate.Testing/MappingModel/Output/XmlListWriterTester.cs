using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlListWriterTester
    {
        private XmlListWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteBatchSizeAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCascadeAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Cascade, "all").MapsToAttribute("cascade");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCheckAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Check, "ck").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCollectionTypeAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.CollectionType, "type").MapsToAttribute("collection-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFetchAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Fetch, "fetch").MapsToAttribute("fetch");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteGenericAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Generic, true).MapsToAttribute("generic");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInverseAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Inverse, true).MapsToAttribute("inverse");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOptimisticLockAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.OptimisticLock, "lock").MapsToAttribute("optimistic-lock");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOuterJoinAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.OuterJoin, "oj").MapsToAttribute("outer-join");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePersisterAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Persister, "p").MapsToAttribute("persister");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTableAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.TableName, "table").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteWhereAttribute()
        {
            writer = new XmlListWriter(null, null);
            var testHelper = new XmlWriterTestHelper<ListMapping>();
            testHelper.Check(x => x.Where, "x = 1").MapsToAttribute("where");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteKey()
        {
            var mapping = new ListMapping
            {
                Key = new KeyMapping()
            };

            writer = new XmlListWriter(new XmlKeyWriter(null), null);
            writer.VerifyXml(mapping)
                .Element("key").Exists();
        }

        [Test]
        public void ShouldWriteRelationshipElement()
        {
            var mapping = new ListMapping();

            mapping.Relationship = new OneToManyMapping();

            writer = new XmlListWriter(null, new XmlCollectionRelationshipWriter(new XmlOneToManyWriter(), null));
            writer.VerifyXml(mapping)
                .Element("one-to-many").Exists();
        }
    }
}