using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCompositeElementWriter : NullMappingModelVisitor, IXmlWriter<CompositeElementMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        protected XmlDocument document;

        public XmlCompositeElementWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(CompositeElementMapping compositeElement)
        {
            document = null;
            compositeElement.AcceptVisitor(this);
            return document;
        }

        public override void ProcessCompositeElement(CompositeElementMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("composite-element");

            if (mapping.HasValue(x => x.Class))
                element.WithAtt("class", mapping.Class);
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            var writer = serviceLocator.GetWriter<PropertyMapping>();
            var xml = writer.Write(propertyMapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(ManyToOneMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ManyToOneMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(ParentMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ParentMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }
    }
}
