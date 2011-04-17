using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlDiscriminatorWriter : NullMappingModelVisitor, IXmlWriter<DiscriminatorMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlDiscriminatorWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(DiscriminatorMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping)
        {
            document = new XmlDocument();

            var discriminatorElement = document.AddElement("discriminator");

            if (discriminatorMapping.IsSpecified("Type"))
                discriminatorElement.WithAtt("type", TypeMapping.GetTypeString(discriminatorMapping.Type.GetUnderlyingSystemType()));

            if (discriminatorMapping.IsSpecified("Force"))
                discriminatorElement.WithAtt("force", discriminatorMapping.Force);

            if (discriminatorMapping.IsSpecified("Formula"))
                discriminatorElement.WithAtt("formula", discriminatorMapping.Formula);

            if (discriminatorMapping.IsSpecified("Insert"))
                discriminatorElement.WithAtt("insert", discriminatorMapping.Insert);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(columnMapping);

            document.ImportAndAppendChild(columnXml);
        }
    }
}