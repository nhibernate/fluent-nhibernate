using System;
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

            if (columnMapping.Attributes.IsSpecified(x => x.Name))
                element.WithAtt("name", columnMapping.Name);

            if (columnMapping.Attributes.IsSpecified(x => x.Check))
                element.WithAtt("check", columnMapping.Check);

            if (columnMapping.Attributes.IsSpecified(x => x.Length))
                element.WithAtt("length", columnMapping.Length);

            if (columnMapping.Attributes.IsSpecified(x => x.Index))
                element.WithAtt("index", columnMapping.Index);

            if (columnMapping.Attributes.IsSpecified(x => x.NotNull))
                element.WithAtt("not-null", columnMapping.NotNull);

            if (columnMapping.Attributes.IsSpecified(x => x.SqlType))
                element.WithAtt("sql-type", columnMapping.SqlType);

            if (columnMapping.Attributes.IsSpecified(x => x.Unique))
                element.WithAtt("unique", columnMapping.Unique);

            if (columnMapping.Attributes.IsSpecified(x => x.UniqueKey))
                element.WithAtt("unique-key", columnMapping.UniqueKey);

            document.AppendChild(element);
        }
    }
}
