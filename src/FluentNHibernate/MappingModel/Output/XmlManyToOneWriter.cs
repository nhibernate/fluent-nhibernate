using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlManyToOneWriter : NullMappingModelVisitor, IXmlWriter<ManyToOneMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlManyToOneWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(ManyToOneMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessManyToOne(ManyToOneMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("many-to-one");

            if (mapping.IsSpecified("Access"))
                element.WithAtt("access", mapping.Access);

            if (mapping.IsSpecified("Cascade"))
                element.WithAtt("cascade", mapping.Cascade);

            if (mapping.IsSpecified("Class"))
                element.WithAtt("class", mapping.Class);

            if (mapping.IsSpecified("Fetch"))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.IsSpecified("ForeignKey"))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.IsSpecified("Insert"))
                element.WithAtt("insert", mapping.Insert);

            if (mapping.IsSpecified("Lazy"))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.IsSpecified("Name"))
                element.WithAtt("name", mapping.Name);

            if (mapping.IsSpecified("NotFound"))
                element.WithAtt("not-found", mapping.NotFound);

            if (mapping.IsSpecified("Formula"))
                element.WithAtt("formula", mapping.Formula);

            if (mapping.IsSpecified("PropertyRef"))
                element.WithAtt("property-ref", mapping.PropertyRef);

            if (mapping.IsSpecified("Update"))
                element.WithAtt("update", mapping.Update);

            if (mapping.IsSpecified("EntityName"))
                element.WithAtt("entity-name", mapping.EntityName);

            if (mapping.IsSpecified("OptimisticLock"))
                element.WithAtt("optimistic-lock", mapping.OptimisticLock);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var xml = writer.Write(columnMapping);

            document.ImportAndAppendChild(xml);
        }
    }
}