﻿using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlClassWriterTester
{
    IXmlWriter<ClassMapping> writer;

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
    public void ShouldWriteSubselectAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ClassMapping>();
        testHelper.Check(x => x.Subselect, "val").MapsToAttribute("subselect");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteSchemaActionAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ClassMapping>();
        testHelper.Check(x => x.SchemaAction, "none").MapsToAttribute("schema-action");

        testHelper.VerifyAll(writer);
    }


    [Test]
    public void ShouldWriteEntityNameAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ClassMapping>();
        testHelper.Check(x => x.EntityName, "entity1").MapsToAttribute("entity-name");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteCache()
    {
        var mapping = new ClassMapping();

        mapping.Set(x => x.Cache, Layer.Defaults, new CacheMapping());

        writer.VerifyXml(mapping)
            .Element("cache").Exists();
    }

    [Test]
    public void ShouldWriteId()
    {
        var mapping = new ClassMapping();

        mapping.Set(x => x.Id, Layer.Defaults, new IdMapping());

        writer.VerifyXml(mapping)
            .Element("id").Exists();
    }

    [Test]
    public void ShouldWriteNaturalId()
    {
        var mapping = new ClassMapping();

        mapping.Set(x => x.NaturalId, Layer.Defaults, new NaturalIdMapping());

        writer.VerifyXml(mapping)
            .Element("natural-id").Exists();
    }

    [Test]
    public void ShouldWriteCompositeId()
    {
        var mapping = new ClassMapping();

        mapping.Set(x => x.Id, Layer.Defaults, new CompositeIdMapping());

        writer.VerifyXml(mapping)
            .Element("composite-id").Exists();
    }

    [Test]
    public void ShouldWriteVersion()
    {
        var mapping = new ClassMapping();

        mapping.Set(x => x.Version, Layer.Defaults, new VersionMapping());

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

        mapping.AddComponent(new ComponentMapping(ComponentType.Component));

        writer.VerifyXml(mapping)
            .Element("component").Exists();
    }

    [Test]
    public void ShouldWriteDynamicComponents()
    {
        var mapping = new ClassMapping();

        mapping.AddComponent(new ComponentMapping(ComponentType.DynamicComponent));

        writer.VerifyXml(mapping)
            .Element("dynamic-component").Exists();
    }

    [Test]
    public void ShouldWriteAny()
    {
        var mapping = new ClassMapping();

        mapping.AddAny(new AnyMapping());

        writer.VerifyXml(mapping)
            .Element("any").Exists();
    }

    [Test]
    public void ShouldWriteMap()
    {
        var mapping = new ClassMapping();

        mapping.AddCollection(CollectionMapping.Map());

        writer.VerifyXml(mapping)
            .Element("map").Exists();
    }

    [Test]
    public void ShouldWriteSet()
    {
        var mapping = new ClassMapping();

        mapping.AddCollection(CollectionMapping.Set());

        writer.VerifyXml(mapping)
            .Element("set").Exists();
    }

    [Test]
    public void ShouldWriteList()
    {
        var mapping = new ClassMapping();

        mapping.AddCollection(CollectionMapping.List());

        writer.VerifyXml(mapping)
            .Element("list").Exists();
    }

    [Test]
    public void ShouldWriteBag()
    {
        var mapping = new ClassMapping();

        mapping.AddCollection(CollectionMapping.Bag());

        writer.VerifyXml(mapping)
            .Element("bag").Exists();
    }

    [Test, Ignore("ShouldWriteIdBag")]
    public void ShouldWriteIdBag()
    {
        Assert.Fail();
    }

    [Test, Ignore("ShouldWriteArray")]
    public void ShouldWriteArray()
    {
        Assert.Fail();
    }

    [Test, Ignore("ShouldWritePrimitiveArray")]
    public void ShouldWritePrimitiveArray()
    {
        Assert.Fail();
    }

    [Test]
    public void ShouldWriteJoin()
    {
        var mapping = new ClassMapping();

        mapping.AddJoin(new JoinMapping());

        writer.VerifyXml(mapping)
            .Element("join").Exists();
    }

    [Test]
    public void ShouldWriteSubclass()
    {
        var mapping = new ClassMapping();

        mapping.AddSubclass(new SubclassMapping(SubclassType.Subclass));

        writer.VerifyXml(mapping)
            .Element("subclass").Exists();
    }

    [Test]
    public void ShouldWriteJoinedSubclass()
    {
        var mapping = new ClassMapping();

        mapping.AddSubclass(new SubclassMapping(SubclassType.JoinedSubclass));

        writer.VerifyXml(mapping)
            .Element("joined-subclass").Exists();
    }

    [Test, Ignore("ShouldWriteUnionSubclass")]
    public void ShouldWriteUnionSubclass()
    {
        Assert.Fail();
    }

    [Test]
    public void ShouldWriteDiscriminator()
    {
        var mapping = new ClassMapping();

        mapping.Set(x => x.Discriminator, Layer.Defaults, new DiscriminatorMapping());

        writer.VerifyXml(mapping)
            .Element("discriminator").Exists();
    }

}
