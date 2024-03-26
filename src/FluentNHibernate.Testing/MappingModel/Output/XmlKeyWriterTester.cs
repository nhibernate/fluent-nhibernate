using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlKeyWriterTester
{
    IXmlWriter<KeyMapping> writer;

    [SetUp]
    public void GetWriterFromContainer()
    {
        var container = new XmlWriterContainer();
        writer = container.Resolve<IXmlWriter<KeyMapping>>();
    }

    [Test]
    public void ShouldWriteForeignKeyAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyMapping>();
        testHelper.Check(x => x.ForeignKey, "fk").MapsToAttribute("foreign-key");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWritePropertyRefAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyMapping>();
        testHelper.Check(x => x.PropertyRef, "prop").MapsToAttribute("property-ref");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteOnDeleteAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyMapping>();
        testHelper.Check(x => x.OnDelete, "cascade").MapsToAttribute("on-delete");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteNotNullAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyMapping>();
        testHelper.Check(x => x.NotNull, true).MapsToAttribute("not-null");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteUpdateAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyMapping>();
        testHelper.Check(x => x.Update, true).MapsToAttribute("update");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteUniqueAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyMapping>();
        testHelper.Check(x => x.Unique, true).MapsToAttribute("unique");

        testHelper.VerifyAll(writer);
    }


    [Test]
    public void ShouldWriteColumns()
    {
        var mapping = new KeyMapping();
        mapping.AddColumn(Layer.Defaults, new ColumnMapping("Column1"));

        writer.VerifyXml(mapping)
            .Element("column").Exists();
    }
}
