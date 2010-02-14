using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Type;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlFilterWriter : NullMappingModelVisitor, IXmlWriter<FilterMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(FilterMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessFilter(FilterMapping filterDefinitionMapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("filter");
            element.WithAtt("name", filterDefinitionMapping.Name);

            if (!string.IsNullOrEmpty(filterDefinitionMapping.Condition))
                element.WithAtt("condition", filterDefinitionMapping.Condition);

            document.AppendChild(element);
        }
    }
}
