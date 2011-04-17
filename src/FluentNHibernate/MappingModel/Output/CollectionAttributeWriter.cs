using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class BaseXmlCollectionWriter : NullMappingModelVisitor
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        protected XmlDocument document;

        protected BaseXmlCollectionWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override void Visit(KeyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<KeyMapping>();
            var keyXml = writer.Write(mapping);

            document.ImportAndAppendChild(keyXml);
        }

        public override void Visit(CacheMapping mapping)
        {
            var writer = serviceLocator.GetWriter<CacheMapping>();
            var cacheXml = writer.Write(mapping);

            document.ImportAndAppendChild(cacheXml);
        }

        public override void Visit(ICollectionRelationshipMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ICollectionRelationshipMapping>();
            var relationshipXml = writer.Write(mapping);

            document.ImportAndAppendChild(relationshipXml);
        }

        public override void Visit(CompositeElementMapping mapping)
        {
            var writer = serviceLocator.GetWriter<CompositeElementMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(ElementMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ElementMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(FilterMapping filterMapping)
        {
            var writer = serviceLocator.GetWriter<FilterMapping>();
            var xml = writer.Write(filterMapping);
            document.ImportAndAppendChild(xml);
        }

        protected void WriteBaseCollectionAttributes(XmlElement element, CollectionMapping mapping)
        {
            if (mapping.IsSpecified("Access"))
                element.WithAtt("access", mapping.Access);

            if (mapping.IsSpecified("BatchSize"))
                element.WithAtt("batch-size", mapping.BatchSize);

            if (mapping.IsSpecified("Cascade"))
                element.WithAtt("cascade", mapping.Cascade);

            if (mapping.IsSpecified("Check"))
                element.WithAtt("check", mapping.Check);

            if (mapping.IsSpecified("CollectionType") && mapping.CollectionType != TypeReference.Empty)
                element.WithAtt("collection-type", mapping.CollectionType);

            if (mapping.IsSpecified("Fetch"))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.IsSpecified("Generic"))
                element.WithAtt("generic", mapping.Generic);

            if (mapping.IsSpecified("Inverse"))
                element.WithAtt("inverse", mapping.Inverse);

            if (mapping.IsSpecified("Lazy"))
                element.WithAtt("lazy", mapping.Lazy.ToString().ToLowerInvariant());

            if (mapping.IsSpecified("Name"))
                element.WithAtt("name", mapping.Name);

            if (mapping.IsSpecified("OptimisticLock"))
                element.WithAtt("optimistic-lock", mapping.OptimisticLock);

            if (mapping.IsSpecified("Persister"))
                element.WithAtt("persister", mapping.Persister);

            if (mapping.IsSpecified("Schema"))
                element.WithAtt("schema", mapping.Schema);

            if (mapping.IsSpecified("TableName"))
                element.WithAtt("table", mapping.TableName);

            if (mapping.IsSpecified("Where"))
                element.WithAtt("where", mapping.Where);

            if (mapping.IsSpecified("Subselect"))
                element.WithAtt("subselect", mapping.Subselect);

            if (mapping.IsSpecified("Mutable"))
                element.WithAtt("mutable", mapping.Mutable);
        }
    }
}