using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlJoinWriter : NullMappingModelVisitor, IXmlWriter<JoinMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlJoinWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(JoinMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessJoin(JoinMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("join");

            if (mapping.IsSpecified("TableName"))
                element.WithAtt("table", mapping.TableName);

            if (mapping.IsSpecified("Schema"))
                element.WithAtt("schema", mapping.Schema);

            if (mapping.IsSpecified("Fetch"))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.IsSpecified("Catalog"))
                element.WithAtt("catalog", mapping.Catalog);

            if (mapping.IsSpecified("Subselect"))
                element.WithAtt("subselect", mapping.Subselect);

            if (mapping.IsSpecified("Inverse"))
                element.WithAtt("inverse", mapping.Inverse);

            if (mapping.IsSpecified("Optional"))
                element.WithAtt("optional", mapping.Optional);
        }

        public override void Visit(PropertyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<PropertyMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(KeyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<KeyMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(ManyToOneMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ManyToOneMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(IComponentMapping mapping)
        {
            var writer = serviceLocator.GetWriter<IComponentMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(AnyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<AnyMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(CollectionMapping mapping)
        {
            var writer = serviceLocator.GetWriter<CollectionMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }
    }
}
