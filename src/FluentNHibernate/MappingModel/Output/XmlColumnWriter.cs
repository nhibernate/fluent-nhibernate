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

            if (columnMapping.IsSpecified(x => x.Name))
                element.WithAtt("name", columnMapping.Name);

            if (columnMapping.IsSpecified(x => x.Check))
                element.WithAtt("check", columnMapping.Check);

            if (columnMapping.IsSpecified(x => x.Length))
                element.WithAtt("length", columnMapping.Length);

            if (columnMapping.IsSpecified(x => x.Index))
                element.WithAtt("index", columnMapping.Index);

            if (columnMapping.IsSpecified(x => x.NotNull))
                element.WithAtt("not-null", columnMapping.NotNull);

            if (columnMapping.IsSpecified(x => x.SqlType))
                element.WithAtt("sql-type", columnMapping.SqlType);

            if (columnMapping.IsSpecified(x => x.Unique))
                element.WithAtt("unique", columnMapping.Unique);

            if (columnMapping.IsSpecified(x => x.UniqueKey))
                element.WithAtt("unique-key", columnMapping.UniqueKey);

            document.AppendChild(element);
        }
    }
}
