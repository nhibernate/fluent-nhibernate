using System.Xml;
using FluentNHibernate.Mapping;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlColumnWriter : NullMappingModelVisitor, IXmlWriter<ColumnMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(ColumnMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("column");

            element.WithAtt("name", columnMapping.Name);

            if (columnMapping.Attributes.IsSpecified(x => x.Length))
                element.WithAtt("length", columnMapping.Length.ToString());

            if (columnMapping.Attributes.IsSpecified(x => x.NotNull))
                element.WithAtt("not-null", columnMapping.NotNull.ToString().ToLowerInvariant());

            if (columnMapping.Attributes.IsSpecified(x => x.SqlType))
                element.WithAtt("sql-type", columnMapping.SqlType);

            if (columnMapping.Attributes.IsSpecified(x => x.Unique))
                element.WithAtt("unique", columnMapping.Unique.ToString().ToLowerInvariant());

            document.AppendChild(element);
        }
    }
}
