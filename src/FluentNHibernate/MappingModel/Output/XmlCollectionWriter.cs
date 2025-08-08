using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlCollectionWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<CollectionMapping>
{
    XmlDocument document;
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
        IXmlWriter<CollectionMapping> writer = mapping.Collection switch
        {
            Collection.Array => new XmlArrayWriter(serviceLocator),
            Collection.Bag => new XmlBagWriter(serviceLocator),
            Collection.List => new XmlListWriter(serviceLocator),
            Collection.Map => new XmlMapWriter(serviceLocator),
            Collection.Set => new XmlSetWriter(serviceLocator),
            Collection.IdBag => new XmlIdBagWriter(serviceLocator),
            _ => throw new InvalidOperationException("Unrecognised collection type " + mapping.Collection)
        };

        document = writer.Write(mapping);
    }
}
