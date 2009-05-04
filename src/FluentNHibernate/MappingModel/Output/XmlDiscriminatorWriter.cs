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

            //if (discriminatorMapping.Attributes.IsSpecified(x => x.ColumnName))
            //    document.SetColumn(discriminatorMapping.ColumnName);

            //if (discriminatorMapping.Attributes.IsSpecified(x => x.Type))
            //    document.type = discriminatorMapping.Type.ToString();

            //if (discriminatorMapping.Attributes.IsSpecified(x => x.Force))
            //    document.force = discriminatorMapping.Force;

            //if (discriminatorMapping.Attributes.IsSpecified(x => x.Formula))
            //    document.formula = discriminatorMapping.Formula;

            //if (discriminatorMapping.Attributes.IsSpecified(x => x.Insert))
            //{
            //    document.SetInsert(discriminatorMapping.Insert);
            //}

            //if (discriminatorMapping.Attributes.IsSpecified(x => x.IsNotNullable))
            //    document.notnull = discriminatorMapping.IsNotNullable;

            //if (discriminatorMapping.Attributes.IsSpecified(x => x.Length))
            //    document.length = discriminatorMapping.Length.ToString();
        }

        //public override void Visit(ColumnMapping columnMapping)
        //{
        //    document.SetColumn((HbmColumn)_columnWriter.Write(columnMapping));
        //}
    }
}