using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output;

[TestFixture]
public class XmlFilterDefinitionWriterTester
{
    [Test]
    public void ShouldWriteTheFilterDefinitions()
    {
        var filterDefinition = new FilterDefinitionMapping();
        filterDefinition.Set(x => x.Name, Layer.Defaults, "sid");
        filterDefinition.Parameters.Add("george", NHibernateUtil.Int32);
            
        var writer = new XmlFilterDefinitionWriter();
        writer.VerifyXml(filterDefinition)
            .RootElement.HasName("filter-def")
            .HasAttribute("name", "sid")
            .Element("filter-param")
            .HasAttribute("name", "george")
            .HasAttribute("type", "Int32");
    }
}
