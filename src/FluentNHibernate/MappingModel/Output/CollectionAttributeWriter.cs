using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

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

        protected void WriteBaseCollectionAttributes(XmlElement element, ICollectionMapping mapping)
        {
            if (mapping.HasValue(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.HasValue(x => x.BatchSize))
                element.WithAtt("batch-size", mapping.BatchSize);

            if (mapping.HasValue(x => x.Cascade))
                element.WithAtt("cascade", mapping.Cascade);

            if (mapping.HasValue(x => x.Check))
                element.WithAtt("check", mapping.Check);

            if (mapping.HasValue(x => x.CollectionType))
                element.WithAtt("collection-type", mapping.CollectionType);

            if (mapping.HasValue(x => x.Fetch))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.HasValue(x => x.Generic))
                element.WithAtt("generic", mapping.Generic);

            if (mapping.HasValue(x => x.Inverse))
                element.WithAtt("inverse", mapping.Inverse);

            if (mapping.HasValue(x => x.Lazy))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.HasValue(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.HasValue(x => x.OptimisticLock))
                element.WithAtt("optimistic-lock", mapping.OptimisticLock);

            if (mapping.HasValue(x => x.Persister))
                element.WithAtt("persister", mapping.Persister);

            if (mapping.HasValue(x => x.Schema))
                element.WithAtt("schema", mapping.Schema);

            if (mapping.HasValue(x => x.TableName))
                element.WithAtt("table", mapping.TableName);

            if (mapping.HasValue(x => x.Where))
                element.WithAtt("where", mapping.Where);

            if (mapping.HasValue(x => x.Subselect))
                element.WithAtt("subselect", mapping.Subselect);

            if (mapping.HasValue(x => x.Mutable))
                element.WithAtt("mutable", mapping.Mutable);
        }
    }
}