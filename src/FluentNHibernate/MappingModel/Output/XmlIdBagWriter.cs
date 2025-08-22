using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output;

public class XmlIdBagWriter(IXmlWriterServiceLocator serviceLocator) : BaseXmlCollectionWriter(serviceLocator), IXmlWriter<CollectionMapping>
{
    public XmlDocument Write(CollectionMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
        document = new XmlDocument();
        var element = document.AddElement("idbag");
        WriteBaseCollectionAttributes(element, mapping);
    }
    
    public override void Visit(CollectionIdMapping mapping)
    {
        var writer = serviceLocator.GetWriter<CollectionIdMapping>();
        var xml = writer.Write(mapping);
        document.ImportAndAppendChild(xml);
    }
    
    public override void Visit(GeneratorMapping mapping)
    {
        var writer = serviceLocator.GetWriter<GeneratorMapping>();
        var generatorXml = writer.Write(mapping);
        document.ImportAndAppendChild(generatorXml);
    }
}
