using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlDynamicComponentWriter : BaseXmlComponentWriter, IXmlWriter<DynamicComponentMapping>
    {
        public XmlDynamicComponentWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {}

        public XmlDocument Write(DynamicComponentMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessComponent(DynamicComponentMapping componentMapping)
        {
            document = WriteComponent("dynamic-component", componentMapping);
        }
    }
}