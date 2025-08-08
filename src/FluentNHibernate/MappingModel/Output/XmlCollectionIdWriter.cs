using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlCollectionIdWriter(IXmlWriterServiceLocator serviceLocator) : NullMappingModelVisitor, IXmlWriter<CollectionIdMapping>
{
    XmlDocument document;

    public XmlDocument Write(CollectionIdMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessCollectionId(CollectionIdMapping mapping)
    {
        document = new XmlDocument();

        var element = document.AddElement("collection-id");

        if (mapping.IsSpecified("Column"))
            element.WithAtt("column", mapping.Column);

        if (mapping.IsSpecified("Type"))
            element.WithAtt("type", mapping.Type);

        if (mapping.IsSpecified("Length"))
            element.WithAtt("length", mapping.Length);
    }

    public override void Visit(GeneratorMapping mapping)
    {
        var writer = serviceLocator.GetWriter<GeneratorMapping>();
        var generatorXml = writer.Write(mapping);

        document.ImportAndAppendChild(generatorXml);
    }
}
