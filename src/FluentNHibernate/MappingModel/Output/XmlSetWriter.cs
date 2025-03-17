using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output;

public class XmlSetWriter(IXmlWriterServiceLocator serviceLocator)
    : BaseXmlCollectionWriter(serviceLocator), IXmlWriter<CollectionMapping>
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

        var element = document.AddElement("set");

        WriteBaseCollectionAttributes(element, mapping);

        if (mapping.IsSpecified("OrderBy"))
            element.WithAtt("order-by", mapping.OrderBy);

        if (mapping.IsSpecified("Sort"))
            element.WithAtt("sort", mapping.Sort);
    }
}
