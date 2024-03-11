﻿using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlJoinedSubclassWriterTester
{
    private IXmlWriter<SubclassMapping> writer;

    private XmlWriterTestHelper<SubclassMapping> create_helper()
    {
        var helper = new XmlWriterTestHelper<SubclassMapping>();
        helper.CreateInstance(() => new SubclassMapping(SubclassType.JoinedSubclass));
        return helper;
    }

    [SetUp]
    public void GetWriterFromContainer()
    {
        var container = new XmlWriterContainer();
        writer = container.Resolve<IXmlWriter<SubclassMapping>>();
    }

    [Test]
    public void ShouldWriteTableAttribute()
    {
        var testHelper = create_helper();
        testHelper.Check(x => x.TableName, "tbl").MapsToAttribute("table");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteSchemaAttribute()
    {
        var testHelper = create_helper();
        testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteNameAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteProxyAttribute()
    {
        var testHelper = create_helper();
        testHelper.Check(x => x.Proxy, "p").MapsToAttribute("proxy");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteCheckAttribute()
    {
        var testHelper = create_helper();
        testHelper.Check(x => x.Check, "chk").MapsToAttribute("check");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteLazyAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteDynamicUpdateAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.DynamicUpdate, true).MapsToAttribute("dynamic-update");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteDynamicInsertAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.DynamicInsert, true).MapsToAttribute("dynamic-insert");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteSelectBeforeUpdateAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.SelectBeforeUpdate, true).MapsToAttribute("select-before-update");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteAbstractAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.Abstract, true).MapsToAttribute("abstract");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteSubselectAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.Subselect, "subselect").MapsToAttribute("subselect");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWritePersisterAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.Persister, new TypeReference(typeof(string))).MapsToAttribute("persister");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteBatchSizeAttribute()
    {

        var testHelper = create_helper();
        testHelper.Check(x => x.BatchSize, 10).MapsToAttribute("batch-size");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteEntityNameAttribute()
    {
        var testHelper = create_helper();
        testHelper.Check(x => x.EntityName, "entity1").MapsToAttribute("entity-name");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteProperties()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddProperty(new PropertyMapping());

        writer.VerifyXml(mapping)
            .Element("property").Exists();
    }

    [Test]
    public void ShouldWriteManyToOnes()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddReference(new ManyToOneMapping());

        writer.VerifyXml(mapping)
            .Element("many-to-one").Exists();
    }

    [Test]
    public void ShouldWriteOneToOnes()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddOneToOne(new OneToOneMapping());

        writer.VerifyXml(mapping)
            .Element("one-to-one").Exists();
    }

    [Test]
    public void ShouldWriteComponents()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddComponent(new ComponentMapping(ComponentType.Component));

        writer.VerifyXml(mapping)
            .Element("component").Exists();
    }

    [Test]
    public void ShouldWriteDynamicComponents()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddComponent(new ComponentMapping(ComponentType.DynamicComponent));

        writer.VerifyXml(mapping)
            .Element("dynamic-component").Exists();
    }

    [Test]
    public void ShouldWriteAny()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddAny(new AnyMapping());

        writer.VerifyXml(mapping)
            .Element("any").Exists();
    }

    [Test]
    public void ShouldWriteMap()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddCollection(CollectionMapping.Map());

        writer.VerifyXml(mapping)
            .Element("map").Exists();
    }

    [Test]
    public void ShouldWriteSet()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddCollection(CollectionMapping.Set());

        writer.VerifyXml(mapping)
            .Element("set").Exists();
    }

    [Test]
    public void ShouldWriteList()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddCollection(CollectionMapping.List());

        writer.VerifyXml(mapping)
            .Element("list").Exists();
    }

    [Test]
    public void ShouldWriteBag()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddCollection(CollectionMapping.Bag());

        writer.VerifyXml(mapping)
            .Element("bag").Exists();
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
    public void ShouldWriteSubclass()
    {
        var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

        mapping.AddSubclass(new SubclassMapping(SubclassType.JoinedSubclass));

        writer.VerifyXml(mapping)
            .Element("joined-subclass").Exists();
    }
}
