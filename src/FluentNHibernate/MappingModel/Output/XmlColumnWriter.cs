using System.Xml;
using FluentNHibernate.Utils;

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

            if (columnMapping.HasValue(x => x.Name))
                element.WithAtt("name", columnMapping.Name);

            if (columnMapping.HasValue(x => x.Check))
                element.WithAtt("check", columnMapping.Check);

            if (columnMapping.HasValue(x => x.Length))
                element.WithAtt("length", columnMapping.Length);

            if (columnMapping.HasValue(x => x.Index))
                element.WithAtt("index", columnMapping.Index);

            if (columnMapping.HasValue(x => x.NotNull))
                element.WithAtt("not-null", columnMapping.NotNull);

            if (columnMapping.HasValue(x => x.SqlType))
                element.WithAtt("sql-type", columnMapping.SqlType);

            if (columnMapping.HasValue(x => x.Unique))
                element.WithAtt("unique", columnMapping.Unique);

            if (columnMapping.HasValue(x => x.UniqueKey))
                element.WithAtt("unique-key", columnMapping.UniqueKey);

            if (columnMapping.HasValue(x => x.Precision))
                element.WithAtt("precision", columnMapping.Precision);

            if (columnMapping.HasValue(x => x.Scale))
                element.WithAtt("scale", columnMapping.Scale);

            document.AppendChild(element);
        }
    }
}
