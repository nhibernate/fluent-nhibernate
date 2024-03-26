using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlIndexManyToManyWriterTester
{
    IXmlWriter<IndexManyToManyMapping> writer;

    [SetUp]
    public void GetWriterFromContainer()
    {
        var container = new XmlWriterContainer();
        writer = container.Resolve<IXmlWriter<IndexManyToManyMapping>>();
    }

    [Test]
    public void ShouldWriteTypeAttribute()
    {
        var testHelper = new XmlWriterTestHelper<IndexManyToManyMapping>();

        testHelper.Check(x => x.Class, new TypeReference("cls")).MapsToAttribute("class");
        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteForeignKey()
    {
        var mapping = new IndexManyToManyMapping();

        mapping.Set(x => x.ForeignKey, Layer.Defaults, "FKTest");

        writer.VerifyXml(mapping)
            .HasAttribute("foreign-key","FKTest");
    }

    [Test]
    public void ShouldWriteColumns()
    {
        var mapping = new IndexManyToManyMapping();

        mapping.AddColumn(Layer.Defaults, new ColumnMapping());

        writer.VerifyXml(mapping)
            .Element("column").Exists();
    }

    [Test]
    public void ShouldWriteEntityName()
    {
        var testHelper = new XmlWriterTestHelper<IndexManyToManyMapping>();
        testHelper.Check(x => x.EntityName, "name1").MapsToAttribute("entity-name");

        testHelper.VerifyAll(writer);
    }
}
