using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlKeyPropertyWriterTester
{
    IXmlWriter<KeyPropertyMapping> writer;

    [SetUp]
    public void GetWriterFromContainer()
    {
        var container = new XmlWriterContainer();
        writer = container.Resolve<IXmlWriter<KeyPropertyMapping>>();
    }

    [Test]
    public void ShouldWriteAccessAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyPropertyMapping>();
        testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteNameAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyPropertyMapping>();
        testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteTypeAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyPropertyMapping>();
        testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");

        testHelper.VerifyAll(writer);
    }

    [Test]
    public void ShouldWriteColumns()
    {
        var mapping = new KeyPropertyMapping();

        mapping.AddColumn(new ColumnMapping());

        writer.VerifyXml(mapping)
            .Element("column").Exists();
    }

    [Test]
    public void ShouldWriteLengthAttribute()
    {
        var testHelper = new XmlWriterTestHelper<KeyPropertyMapping>();
        testHelper.Check(x => x.Length, 8).MapsToAttribute("length");

        testHelper.VerifyAll(writer);
    }

    [Test(Description = "Bug #231 :: Bug when trying to change varchar length for CompositeId")]
    public void WhenKeyPropertyMappingContainsLengthAndCustomColumnLengthAttributeIsRenderedIntoColumnElement()
    {
        var keyPropertyMapping = new KeyPropertyMapping();
        keyPropertyMapping.Set(propertyMapping => propertyMapping.Length, Layer.Defaults, 2);
        keyPropertyMapping.AddColumn(new ColumnMapping());
        writer.VerifyXml(keyPropertyMapping).DoesntHaveAttribute("length").Element("column").HasAttribute("length", "2");
    }
}
