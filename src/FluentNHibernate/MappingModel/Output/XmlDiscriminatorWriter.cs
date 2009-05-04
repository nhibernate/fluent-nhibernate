using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlDiscriminatorWriter : NullMappingModelVisitor, IXmlWriter<DiscriminatorMapping>
    {
        private XmlDocument document;

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