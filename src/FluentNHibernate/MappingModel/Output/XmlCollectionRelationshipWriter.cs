using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlCollectionRelationshipWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<ICollectionRelationshipMapping>
{
    private XmlDocument document;

    public XmlDocument Write(ICollectionRelationshipMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessOneToMany(OneToManyMapping mapping)
    {
        var writer = serviceLocator.GetWriter<OneToManyMapping>();
        document = writer.Write(mapping);
    }

    public override void ProcessManyToMany(ManyToManyMapping mapping)
    {
        var writer = serviceLocator.GetWriter<ManyToManyMapping>();
        document = writer.Write(mapping);
    }
}
