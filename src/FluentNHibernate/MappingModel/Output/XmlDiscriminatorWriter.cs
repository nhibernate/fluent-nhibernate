using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlDiscriminatorWriter : NullMappingModelVisitor, IXmlWriter<DiscriminatorMapping>
    {
        private readonly IXmlWriter<ColumnMapping> _columnWriter;
        private XmlDocument document;

        public XmlDiscriminatorWriter(IXmlWriter<ColumnMapping> columnWriter)
        {
            _columnWriter = columnWriter;
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

            var typeString = TypeMapping.GetTypeString(discriminatorMapping.Type);

            document.AddElement("discriminator")
                .WithAtt("column", discriminatorMapping.ColumnName)
                .WithAtt("type", typeString);
        }
    }
}