using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlCollectionWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<CollectionMapping>
{
    private XmlDocument document;
    Collection collection;

    public XmlDocument Write(CollectionMapping mappingModel)
    {
        collection = mappingModel.Collection;

        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
        IXmlWriter<CollectionMapping> writer = null;

        switch (mapping.Collection)
        {
            case Collection.Array:
                writer = new XmlArrayWriter(serviceLocator);
                break;
            case Collection.Bag:
                writer = new XmlBagWriter(serviceLocator);
                break;
            case Collection.List:
                writer = new XmlListWriter(serviceLocator);
                break;
            case Collection.Map:
                writer = new XmlMapWriter(serviceLocator);
                break;
            case Collection.Set:
                writer = new XmlSetWriter(serviceLocator);
                break;
            default:
                throw new InvalidOperationException("Unrecognised collection type " + mapping.Collection);
        }

        document = writer.Write(mapping);
    }
}
