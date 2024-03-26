using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlKeyPropertyWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<KeyPropertyMapping>
{
    XmlDocument document;

    public XmlDocument Write(KeyPropertyMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessKeyProperty(KeyPropertyMapping mapping)
    {
        document = new XmlDocument();

        var element = document.AddElement("key-property");

        if (mapping.IsSpecified("Name"))
            element.WithAtt("name", mapping.Name);

        if (mapping.IsSpecified("Access"))
            element.WithAtt("access", mapping.Access);

        if (mapping.IsSpecified("Type"))
            element.WithAtt("type", mapping.Type);

        if (mapping.IsSpecified("Length"))
        {
            if (mapping.Columns.Any())
            {
                foreach (var columnMapping in mapping.Columns.Where(column => !column.IsSpecified("Length")))
                {
                    columnMapping.Set(map => map.Length, Layer.Defaults, mapping.Length);
                }   
            }
            else
            {
                element.WithAtt("length", mapping.Length);
            }
        }
    }

    public override void Visit(ColumnMapping columnMapping)
    {
        var writer = serviceLocator.GetWriter<ColumnMapping>();
        var xml = writer.Write(columnMapping);

        document.ImportAndAppendChild(xml);
    }
}
