using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlArrayWriterTester
{
    IXmlWriter<CollectionMapping> writer;

    [SetUp]
    public void GetWriterFromContainer()
    {
        var container = new XmlWriterContainer();
        writer = container.Resolve<IXmlWriter<CollectionMapping>>();
    }

    [Test]
    public void ShouldWriteAccessAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Access, "acc").MapsToAttribute("access");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteBatchSizeAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteCascadeAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Cascade, "all").MapsToAttribute("cascade");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteCheckAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Check, "ck").MapsToAttribute("check");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteCollectionTypeAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.CollectionType, new TypeReference("type")).MapsToAttribute("collection-type");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldNotWriteCollectionTypeWhenEmpty()
    {
        var mapping = CollectionMapping.Array();
        mapping.Set(x => x.CollectionType, Layer.Defaults, TypeReference.Empty);
        writer.VerifyXml(mapping)
            .DoesntHaveAttribute("collection-type");
    }

    [Test]
    public void ShouldWriteFetchAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Fetch, "fetch").MapsToAttribute("fetch");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteGenericAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Generic, true).MapsToAttribute("generic");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteInverseAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Inverse, true).MapsToAttribute("inverse");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteLazyAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Lazy, Lazy.True).MapsToAttribute("lazy");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteNameAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteOptimisticLockAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.OptimisticLock, true).MapsToAttribute("optimistic-lock");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWritePersisterAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Persister, new TypeReference(typeof(string))).MapsToAttribute("persister");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteSchemaAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteTableAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.TableName, "table").MapsToAttribute("table");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteWhereAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Where, "x = 1").MapsToAttribute("where");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteSubselectAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Subselect, "x = 1").MapsToAttribute("subselect");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteMutableAttribute()
    {
        var testHelper = Helper();
        testHelper.Check(x => x.Mutable, true).MapsToAttribute("mutable");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteKey()
    {
        var mapping = CollectionMapping.Array();
        
        mapping.Set(x => x.Key, Layer.Defaults, new KeyMapping());

        writer.VerifyXml(mapping)
            .Element("key").Exists();
    }

    [Test]
    public void ShouldWriteRelationshipElement()
    {
        var mapping = CollectionMapping.Array();

        mapping.Set(x => x.Relationship, Layer.Defaults, new OneToManyMapping());

        writer.VerifyXml(mapping)
            .Element("one-to-many").Exists();
    }

    [Test]
    public void ShouldWriteCacheElement()
    {
        var mapping = CollectionMapping.Array();

        mapping.Set(x => x.Cache, Layer.Defaults, new CacheMapping());

        writer.VerifyXml(mapping)
            .Element("cache").Exists();
    }

    [Test]
    public void ShouldWriteCompositeElement()
    {
        var mapping = CollectionMapping.Array();

        mapping.Set(x => x.CompositeElement, Layer.Defaults, new CompositeElementMapping());

        writer.VerifyXml(mapping)
            .Element("composite-element").Exists();
    }

    [Test]
    public void ShouldWriteIndexElement()
    {
        var mapping = CollectionMapping.Array();

        mapping.Set(x => x.Index, Layer.Defaults, new IndexMapping());

        writer.VerifyXml(mapping)
            .Element("index").Exists();
    }

    [Test]
    public void ShouldWriteElement()
    {
        var mapping = CollectionMapping.Array();

        mapping.Set(x => x.Element, Layer.Defaults, new ElementMapping());

        writer.VerifyXml(mapping)
            .Element("element").Exists();
    }

    static XmlWriterTestHelper<CollectionMapping> Helper()
    {
        var helper =  new XmlWriterTestHelper<CollectionMapping>();

        helper.CreateInstance(CollectionMapping.Array);

        return helper;
    }
}
