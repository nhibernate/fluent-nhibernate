using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlReferenceComponentWriter : BaseXmlComponentWriter, IXmlWriter<ReferenceComponentMapping>
    {
        private IXmlWriter<IComponentMapping> innerWriter;

        public XmlReferenceComponentWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            innerWriter = serviceLocator.GetWriter<IComponentMapping>();
        }

        public XmlDocument Write(ReferenceComponentMapping mappingModel)
        {
            return innerWriter.Write(mappingModel.MergedModel);
        }
    }

    public class XmlComponentWriter : BaseXmlComponentWriter, IXmlWriter<IComponentMapping>
    {
        public XmlComponentWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {}

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
}
