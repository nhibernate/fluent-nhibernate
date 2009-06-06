using System;
using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlComponentBaseWriter : NullMappingModelVisitor, IXmlWriter<IComponentMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlComponentBaseWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(IComponentMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessComponent(ComponentMapping componentMapping)
        {
            var writer = serviceLocator.GetWriter<ComponentMapping>();

            document = writer.Write(componentMapping);
        }

        public override void ProcessComponent(DynamicComponentMapping componentMapping)
        {
            var writer = serviceLocator.GetWriter<DynamicComponentMapping>();

            document = writer.Write(componentMapping);
        }
    }
}