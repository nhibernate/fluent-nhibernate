using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlManyToManyWriter : NullMappingModelVisitor, IXmlWriter<ManyToManyMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlManyToManyWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(ManyToManyMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessManyToMany(ManyToManyMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("many-to-many");

            if (mapping.IsSpecified("Class"))
                element.WithAtt("class", mapping.Class);

            if (mapping.IsSpecified("Fetch"))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.IsSpecified("ForeignKey"))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.IsSpecified("ChildPropertyRef"))
                element.WithAtt("property-ref", mapping.ChildPropertyRef);

            if (mapping.IsSpecified("Lazy"))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.IsSpecified("NotFound"))
                element.WithAtt("not-found", mapping.NotFound);

            if (mapping.IsSpecified("Where"))
                element.WithAtt("where", mapping.Where);

            if (mapping.IsSpecified("EntityName"))
                element.WithAtt("entity-name", mapping.EntityName);

            if (mapping.IsSpecified("OrderBy"))
                element.WithAtt("order-by", mapping.OrderBy);

        }

        public override void Visit(ColumnMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(mapping);

            document.ImportAndAppendChild(columnXml);
        }

        public override void Visit(FilterMapping filterMapping)
        {
            var writer = serviceLocator.GetWriter<FilterMapping>();
            var xml = writer.Write(filterMapping);
            document.ImportAndAppendChild(xml);
        }
    }
}