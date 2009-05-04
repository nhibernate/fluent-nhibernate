using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlDynamicComponentWriter : XmlClasslikeWriterBase, IXmlWriter<DynamicComponentMapping>
    {
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        private readonly IXmlWriter<ParentMapping> parentWriter;

        public XmlDynamicComponentWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ParentMapping> parentWriter)
            : base(propertyWriter)
        {
            this.propertyWriter = propertyWriter;
            this.parentWriter = parentWriter;
        }

        public XmlDocument Write(DynamicComponentMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessDynamicComponent(DynamicComponentMapping componentMapping)
        {
            document = new XmlDocument();

            var dynamicComponentElement = document.AddElement("dynamic-component");

            if (componentMapping.Attributes.IsSpecified(x => x.Name))
                dynamicComponentElement.WithAtt("name", componentMapping.Name);

            var sortedUnmigratedParts = new List<IMappingPart>(componentMapping.UnmigratedParts);

            sortedUnmigratedParts.Sort(new MappingPartComparer(componentMapping.UnmigratedParts));

            foreach (var part in sortedUnmigratedParts)
            {
                part.Write(dynamicComponentElement, null);
            }

            foreach (var attribute in componentMapping.UnmigratedAttributes)
            {
                dynamicComponentElement.WithAtt(attribute.Key, attribute.Value);
            }
        }

        public override void Visit(DynamicComponentMapping componentMapping)
        {
            var dynamicComponentWriter = new XmlDynamicComponentWriter(propertyWriter, parentWriter);
            var dynamicComponentXml = dynamicComponentWriter.Write(componentMapping);

            document.ImportAndAppendChild(dynamicComponentXml);
        }

        public override void Visit(ParentMapping parentMapping)
        {
            var parentXml = parentWriter.Write(parentMapping);

            document.ImportAndAppendChild(parentXml);
        }
    }
}