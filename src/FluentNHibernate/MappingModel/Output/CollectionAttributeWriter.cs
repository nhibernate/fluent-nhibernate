using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class BaseXmlCollectionWriter : NullMappingModelVisitor
    {
        protected XmlDocument document;
        private readonly IXmlWriter<KeyMapping> keyWriter;

        protected BaseXmlCollectionWriter(IXmlWriter<KeyMapping> keyWriter)
        {
            this.keyWriter = keyWriter;
        }

        public override void Visit(KeyMapping mapping)
        {
            var keyXml = keyWriter.Write(mapping);

            document.ImportAndAppendChild(keyXml);
        }

        protected void WriteBaseCollectionAttributes(XmlElement element, ICollectionMapping mapping)
        {
            if (mapping.Attributes.IsSpecified(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.Attributes.IsSpecified(x => x.BatchSize))
                element.WithAtt("batch-size", mapping.BatchSize);

            if (mapping.Attributes.IsSpecified(x => x.Cascade))
                element.WithAtt("cascade", mapping.Cascade);

            if (mapping.Attributes.IsSpecified(x => x.Check))
                element.WithAtt("check", mapping.Check);

            if (mapping.Attributes.IsSpecified(x => x.CollectionType))
                element.WithAtt("collection-type", mapping.CollectionType);

            if (mapping.Attributes.IsSpecified(x => x.Fetch))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.Attributes.IsSpecified(x => x.Generic))
                element.WithAtt("generic", mapping.Generic);

            if (mapping.Attributes.IsSpecified(x => x.Inverse))
                element.WithAtt("inverse", mapping.Inverse);

            if (mapping.Attributes.IsSpecified(x => x.Lazy))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.Attributes.IsSpecified(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.Attributes.IsSpecified(x => x.OptimisticLock))
                element.WithAtt("optimistic-lock", mapping.OptimisticLock);

            if (mapping.Attributes.IsSpecified(x => x.OuterJoin))
                element.WithAtt("outer-join", mapping.OuterJoin);

            if (mapping.Attributes.IsSpecified(x => x.Persister))
                element.WithAtt("persister", mapping.Persister);

            if (mapping.Attributes.IsSpecified(x => x.Schema))
                element.WithAtt("schema", mapping.Schema);

            if (mapping.Attributes.IsSpecified(x => x.TableName))
                element.WithAtt("table", mapping.TableName);

            if (mapping.Attributes.IsSpecified(x => x.Where))
                element.WithAtt("where", mapping.Where);
        }
    }
}