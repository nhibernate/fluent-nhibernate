using System;
using System.Xml;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCompositeIdWriter : NullMappingModelVisitor, IXmlWriter<CompositeIdMapping>
    {
        private IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlCompositeIdWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(CompositeIdMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessCompositeId(CompositeIdMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("composite-id");

            if (mapping.HasValue(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.HasValue(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.HasValue(x => x.Class))
                element.WithAtt("class", mapping.Class);

            if (mapping.HasValue(x => x.Mapped))
                element.WithAtt("mapped", mapping.Mapped);

            if (mapping.HasValue(x => x.UnsavedValue))
                element.WithAtt("unsaved-value", mapping.UnsavedValue);
        }

        public override void Visit(KeyPropertyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<KeyPropertyMapping>();
            var xml = writer.Write(mapping);

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