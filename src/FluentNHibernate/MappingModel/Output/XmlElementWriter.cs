using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlElementWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<ElementMapping>
{
    private XmlDocument document;

    public XmlDocument Write(ElementMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessElement(ElementMapping mapping)
    {
        document = new XmlDocument();

        var element = document.AddElement("element");

        if (mapping.IsSpecified("Type"))
            element.WithAtt("type", mapping.Type);

        if (mapping.IsSpecified("Formula"))
            element.WithAtt("formula", mapping.Formula);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
        var writer = serviceLocator.GetWriter<ColumnMapping>();
        var xml = writer.Write(columnMapping);

        document.ImportAndAppendChild(xml);
    }
}
