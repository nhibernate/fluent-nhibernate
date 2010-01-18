using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlReferenceComponentWriter : BaseXmlComponentWriter, IXmlWriter<ReferenceComponentMapping>
    {
        private IXmlWriter<ComponentMapping> innerWriter;

        public XmlReferenceComponentWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            innerWriter = serviceLocator.GetWriter<ComponentMapping>();
        }

        public XmlDocument Write(ReferenceComponentMapping mappingModel)
        {
            return innerWriter.Write(mappingModel.MergedModel);
        }
    }

    public class XmlComponentWriter : BaseXmlComponentWriter, IXmlWriter<ComponentMapping>
    {
        public XmlComponentWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {}

        public XmlDocument Write(ComponentMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessComponent(ComponentMapping mapping)
        {
            document = WriteComponent("component", mapping);

            if (mapping.HasValue(x => x.Class))
                document.DocumentElement.WithAtt("class", mapping.Class);

            if (mapping.HasValue(x => x.Lazy))
                document.DocumentElement.WithAtt("lazy", mapping.Lazy);
        }
    }
}
