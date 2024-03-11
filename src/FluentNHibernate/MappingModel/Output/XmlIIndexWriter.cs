using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlIIndexWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<IIndexMapping>
{
    private XmlDocument document;

    public XmlDocument Write(IIndexMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessIndex(IndexMapping mapping)
    {
        var writer = serviceLocator.GetWriter<IndexMapping>();
        document = writer.Write(mapping);
    }

    public override void ProcessIndex(IndexManyToManyMapping mapping)
    {
        var writer = serviceLocator.GetWriter<IndexManyToManyMapping>();
        document = writer.Write(mapping);
    }
}
