using System.Xml;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlIdentityBasedWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<IIdentityMapping>
{
    XmlDocument document;

    public XmlDocument Write(IIdentityMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessId(IdMapping mapping)
    {
        var writer = serviceLocator.GetWriter<IdMapping>();
        document = writer.Write(mapping);
    }

    public override void ProcessCompositeId(CompositeIdMapping idMapping)
    {
        var writer = serviceLocator.GetWriter<CompositeIdMapping>();
        document = writer.Write(idMapping);
    }
}
