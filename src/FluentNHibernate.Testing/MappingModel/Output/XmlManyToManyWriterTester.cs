using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlManyToManyWriterTester
{
    IXmlWriter<ManyToManyMapping> writer;

    [SetUp]
    public void GetWriterFromContainer()
    {
        var container = new XmlWriterContainer();
        writer = container.Resolve<IXmlWriter<ManyToManyMapping>>();
    }

    [Test]
    public void ShouldWriteClassAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.Class, new TypeReference("type")).MapsToAttribute("class");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteFetchAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.Fetch, "f").MapsToAttribute("fetch");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteForeignKeyAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.ForeignKey, "fk").MapsToAttribute("foreign-key");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteLazyAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteNotFoundAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.NotFound, "exception").MapsToAttribute("not-found");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteWhereAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.Where, "x = 1").MapsToAttribute("where");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteColumns()
    {
        var container = new XmlWriterContainer();
        var mapping = new ManyToManyMapping();

        mapping.AddColumn(Layer.Defaults, new ColumnMapping("Column1"));

        writer = container.Resolve<IXmlWriter<ManyToManyMapping>>();
        writer.VerifyXml(mapping)
            .Element("column").Exists();
    }

    [Test]
    public void ShouldWriteEntityName()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.EntityName, "name1").MapsToAttribute("entity-name");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteOrderByAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.OrderBy, "col1").MapsToAttribute("order-by");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWritePropertyRef()
    {
        var testHelper = new XmlWriterTestHelper<ManyToManyMapping>();
        testHelper.Check(x => x.ChildPropertyRef, "childprop").MapsToAttribute("property-ref");

        testHelper.VerifyAll(writer);
    }
}
