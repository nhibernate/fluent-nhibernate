using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlAnyWriter : NullMappingModelVisitor, IXmlWriter<AnyMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlAnyWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(AnyMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessAny(AnyMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("any");

            if (mapping.HasValue(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.HasValue(x => x.Cascade))
                element.WithAtt("cascade", mapping.Cascade);

            if (mapping.HasValue(x => x.IdType))
                element.WithAtt("id-type", mapping.IdType);

            if (mapping.HasValue(x => x.Insert))
                element.WithAtt("insert", mapping.Insert);

            if (mapping.HasValue(x => x.MetaType))
                element.WithAtt("meta-type", mapping.MetaType);

            if (mapping.HasValue(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.HasValue(x => x.Update))
                element.WithAtt("update", mapping.Update);

            if (mapping.HasValue(x => x.Lazy))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.HasValue(x => x.OptimisticLock))
                element.WithAtt("optimistic-lock", mapping.OptimisticLock);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(columnMapping);

            document.ImportAndAppendChild(columnXml);
        }

        public override void Visit(MetaValueMapping mapping)
        {
            var writer = serviceLocator.GetWriter<MetaValueMapping>();
            var metaValueXml = writer.Write(mapping);

            document.ImportAndAppendChild(metaValueXml);
        }
    }
}