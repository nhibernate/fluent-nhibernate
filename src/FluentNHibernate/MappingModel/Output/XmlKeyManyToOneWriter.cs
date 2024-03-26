using System.Xml;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlKeyManyToOneWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<KeyManyToOneMapping>
{
    XmlDocument document;

    public XmlDocument Write(KeyManyToOneMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessKeyManyToOne(KeyManyToOneMapping mapping)
    {
        document = new XmlDocument();

        var element = document.AddElement("key-many-to-one");

        if (mapping.IsSpecified("Access"))
            element.WithAtt("access", mapping.Access);

        if (mapping.IsSpecified("Name"))
            element.WithAtt("name", mapping.Name);

        if (mapping.IsSpecified("Class"))
            element.WithAtt("class", mapping.Class);

        if (mapping.IsSpecified("ForeignKey"))
            element.WithAtt("foreign-key", mapping.ForeignKey);

        if (mapping.IsSpecified("Lazy"))
            element.WithAtt("lazy", mapping.Lazy ? "proxy" : "false");

        if (mapping.IsSpecified("NotFound"))
            element.WithAtt("not-found", mapping.NotFound);

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
