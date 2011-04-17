using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

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

            if (columnMapping.IsSpecified("Name"))
                element.WithAtt("name", columnMapping.Name);

            if (columnMapping.IsSpecified("Check"))
                element.WithAtt("check", columnMapping.Check);

            if (columnMapping.IsSpecified("Length"))
                element.WithAtt("length", columnMapping.Length);

            if (columnMapping.IsSpecified("Index"))
                element.WithAtt("index", columnMapping.Index);

            if (columnMapping.IsSpecified("NotNull"))
                element.WithAtt("not-null", columnMapping.NotNull);

            if (columnMapping.IsSpecified("SqlType"))
                element.WithAtt("sql-type", columnMapping.SqlType);

            if (columnMapping.IsSpecified("Unique"))
                element.WithAtt("unique", columnMapping.Unique);

            if (columnMapping.IsSpecified("UniqueKey"))
                element.WithAtt("unique-key", columnMapping.UniqueKey);

            if (columnMapping.IsSpecified("Precision"))
                element.WithAtt("precision", columnMapping.Precision);

            if (columnMapping.IsSpecified("Scale"))
                element.WithAtt("scale", columnMapping.Scale);

            if (columnMapping.IsSpecified("Default"))
                element.WithAtt("default", columnMapping.Default);

            document.AppendChild(element);
        }
    }
}
