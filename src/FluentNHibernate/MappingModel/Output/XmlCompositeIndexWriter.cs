using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCompositeIndexWriter : NullMappingModelVisitor, IXmlWriter<CompositeIndexMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        protected XmlDocument document;

        public XmlCompositeIndexWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(CompositeIndexMapping compositeElement)
        {
            document = null;
            compositeElement.AcceptVisitor(this);
            return document;
        }

        public override void ProcessCompositeIndex(CompositeIndexMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("composite-index");

            if (mapping.HasValue(x => x.Type))
                element.WithAtt("class", mapping.Type);
        }

        public override void Visit(KeyPropertyMapping propertyMapping)
        {
            var writer = serviceLocator.GetWriter<KeyPropertyMapping>();
            var xml = writer.Write(propertyMapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(KeyManyToOneMapping mapping)
        {
            var writer = serviceLocator.GetWriter<KeyManyToOneMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }
    }
}