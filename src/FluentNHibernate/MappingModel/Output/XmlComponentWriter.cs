using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output;

public class XmlReferenceComponentWriter(IXmlWriterServiceLocator serviceLocator)
    : BaseXmlComponentWriter(serviceLocator), IXmlWriter<ReferenceComponentMapping>
{
    IXmlWriter<IComponentMapping> innerWriter = serviceLocator.GetWriter<IComponentMapping>();

    public XmlDocument Write(ReferenceComponentMapping mappingModel)
    {
        return innerWriter.Write(mappingModel.MergedModel);
    }
}

public class XmlComponentWriter(IXmlWriterServiceLocator serviceLocator)
    : BaseXmlComponentWriter(serviceLocator), IXmlWriter<IComponentMapping>
{
    public XmlDocument Write(IComponentMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessComponent(ComponentMapping mapping)
    {
        document = WriteComponent(mapping.ComponentType.GetElementName(), mapping);

        if (mapping.IsSpecified("Class"))
            document.DocumentElement.WithAtt("class", mapping.Class);

        if (mapping.IsSpecified("Lazy"))
            document.DocumentElement.WithAtt("lazy", mapping.Lazy);
    }
}
