using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlIndexManyToManyWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<IndexManyToManyMapping>
{
    private XmlDocument document;

    public XmlDocument Write(IndexManyToManyMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessIndex(IndexManyToManyMapping mapping)
    {
        document = new XmlDocument();

        var element = document.AddElement("index-many-to-many");

        if (mapping.IsSpecified("Class"))
            element.WithAtt("class", mapping.Class);

        if (mapping.IsSpecified("ForeignKey"))
            element.WithAtt("foreign-key", mapping.ForeignKey);

        if (mapping.IsSpecified("EntityName"))
            element.WithAtt("entity-name", mapping.EntityName);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
        var writer = serviceLocator.GetWriter<ColumnMapping>();
        var xml = writer.Write(columnMapping);

        document.ImportAndAppendChild(xml);
    }
}
