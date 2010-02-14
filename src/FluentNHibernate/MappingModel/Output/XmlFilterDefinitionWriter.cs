using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Type;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlFilterDefinitionWriter : NullMappingModelVisitor, IXmlWriter<FilterDefinitionMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(FilterDefinitionMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessFilterDefinition(FilterDefinitionMapping filterDefinitionMapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("filter-def");
            element.WithAtt("name", filterDefinitionMapping.Name);

            if (!string.IsNullOrEmpty(filterDefinitionMapping.Condition))
                element.WithAtt("condition", filterDefinitionMapping.Condition);

            foreach (var pair in filterDefinitionMapping.Parameters)
            {
                var parameter = document.CreateElement("filter-param");
                parameter.WithAtt("name", pair.Key);
                parameter.WithAtt("type", pair.Value.Name);
                element.AppendChild(parameter);
            }

            document.AppendChild(element);
        }
    }
}
