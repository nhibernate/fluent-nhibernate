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

            var typeString = TypeMapping.GetTypeString(discriminatorMapping.Type.GetUnderlyingSystemType());

            var discriminatorElement = document.AddElement("discriminator")
                .WithAtt("column", discriminatorMapping.Column)
                .WithAtt("type", typeString);

            if (discriminatorMapping.HasValue(x => x.Force))
                discriminatorElement.WithAtt("force", discriminatorMapping.Force);

            if (discriminatorMapping.HasValue(x => x.Formula))
                discriminatorElement.WithAtt("formula", discriminatorMapping.Formula);

            if (discriminatorMapping.HasValue(x => x.Insert))
                discriminatorElement.WithAtt("insert", discriminatorMapping.Insert);

            if (discriminatorMapping.HasValue(x => x.Length))
                discriminatorElement.WithAtt("length", discriminatorMapping.Length);

            if (discriminatorMapping.HasValue(x => x.NotNull))
                discriminatorElement.WithAtt("not-null", discriminatorMapping.NotNull);
        }
    }
}