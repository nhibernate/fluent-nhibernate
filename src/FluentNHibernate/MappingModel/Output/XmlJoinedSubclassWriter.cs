using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlJoinedSubclassWriter : XmlClassWriterBase, IXmlWriter<JoinedSubclassMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;

        public XmlJoinedSubclassWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(JoinedSubclassMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("joined-subclass")
                .WithAtt("name", mapping.Name);

            if (mapping.Attributes.IsSpecified(x => x.TableName))
                element.WithAtt("table", mapping.TableName);

            if (mapping.Attributes.IsSpecified(x => x.Schema))
                element.WithAtt("schema", mapping.Schema);

            if (mapping.Attributes.IsSpecified(x => x.Extends))
                element.WithAtt("extends", mapping.Extends);

            if (mapping.Attributes.IsSpecified(x => x.Check))
                element.WithAtt("check", mapping.Check);

            if (mapping.Attributes.IsSpecified(x => x.Proxy))
                element.WithAtt("proxy", mapping.Proxy);

            if (mapping.Attributes.IsSpecified(x => x.Lazy))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.Attributes.IsSpecified(x => x.DynamicUpdate))
                element.WithAtt("dynamic-update", mapping.DynamicUpdate);

            if (mapping.Attributes.IsSpecified(x => x.DynamicInsert))
                element.WithAtt("dynamic-insert", mapping.DynamicInsert);

            if (mapping.Attributes.IsSpecified(x => x.SelectBeforeUpdate))
                element.WithAtt("select-before-update", mapping.SelectBeforeUpdate);

            if (mapping.Attributes.IsSpecified(x => x.Abstract))
                element.WithAtt("abstract", mapping.Abstract);
        }

        public override void Visit(KeyMapping keyMapping)
        {
            var writer = serviceLocator.GetWriter<KeyMapping>();
            var keyXml = writer.Write(keyMapping);

            document.ImportAndAppendChild(keyXml);
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            var writer = serviceLocator.GetWriter<IComponentMapping>();
            var componentXml = writer.Write(componentMapping);

            document.ImportAndAppendChild(componentXml);
        }

        public override void Visit(ISubclassMapping subclassMapping)
        {
            var writer = serviceLocator.GetWriter<ISubclassMapping>();
            var subclassXml = writer.Write(subclassMapping);

            document.ImportAndAppendChild(subclassXml);
        }
    }
}