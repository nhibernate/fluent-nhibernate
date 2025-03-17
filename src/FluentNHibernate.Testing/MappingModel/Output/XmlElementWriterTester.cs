using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlElementWriterTester
{
    IXmlWriter<ElementMapping> writer;

    [SetUp]
    public void GetWriterFromContainer()
    {
        var container = new XmlWriterContainer();
        writer = container.Resolve<IXmlWriter<ElementMapping>>();
    }

    [Test]
    public void ShouldWriteTypeAttribute()
    {
        var testHelper = new XmlWriterTestHelper<ElementMapping>();

        testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");
        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteColumns()
    {
        var mapping = new ElementMapping();

        mapping.AddColumn(Layer.Defaults, new ColumnMapping());

        writer.VerifyXml(mapping)
            .Element("column").Exists();
    }

    [Test]
    public void ShouldWriteFormula()
    {
        var testHelper = new XmlWriterTestHelper<ElementMapping>();
        testHelper.Check(x => x.Formula, "formula").MapsToAttribute("formula");

        testHelper.VerifyAll(writer);
    }
}
