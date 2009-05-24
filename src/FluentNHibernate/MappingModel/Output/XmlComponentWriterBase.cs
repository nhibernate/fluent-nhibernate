using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class XmlComponentWriterBase<TModel> : XmlClassWriterBase
    {
        protected readonly IXmlWriter<PropertyMapping> propertyWriter;
        protected readonly IXmlWriter<ParentMapping> parentWriter;
        private readonly IXmlWriter<VersionMapping> versionWriter;
        private readonly IXmlWriter<OneToOneMapping> oneToOneWriter;

        protected XmlComponentWriterBase(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ParentMapping> parentWriter, IXmlWriter<VersionMapping> versionWriter, IXmlWriter<OneToOneMapping> oneToOneWriter)
            : base(propertyWriter, versionWriter, oneToOneWriter)
        {
            this.propertyWriter = propertyWriter;
            this.parentWriter = parentWriter;
            this.versionWriter = versionWriter;
            this.oneToOneWriter = oneToOneWriter;
        }

        public override void ProcessComponent(ComponentMappingBase componentMapping)
        {
            document = new XmlDocument();

            var componentElement = document.AddElement(GetXmlElementComponentName());

            if (componentMapping.Attributes.IsSpecified(x => x.Name))
                componentElement.WithAtt("name", componentMapping.Name);

            if (componentMapping.Attributes.IsSpecified(x => x.Insert))
                componentElement.WithAtt("insert", componentMapping.Insert);

            if (componentMapping.Attributes.IsSpecified(x => x.Update))
                componentElement.WithAtt("update", componentMapping.Update);

            var sortedUnmigratedParts = new List<IMappingPart>(componentMapping.UnmigratedParts);

            sortedUnmigratedParts.Sort(new MappingPartComparer(componentMapping.UnmigratedParts));

            foreach (var part in sortedUnmigratedParts)
            {
                part.Write(componentElement, null);
            }

            foreach (var attribute in componentMapping.UnmigratedAttributes)
            {
                componentElement.WithAtt(attribute.Key, attribute.Value);
            }
        }

        private string GetXmlElementComponentName()
        {
            if (typeof(TModel) == typeof(DynamicComponentMapping))
                return "dynamic-component";
            if (typeof(TModel) == typeof(ComponentMapping))
                return "component";

            throw new NotSupportedException("The chosen component type is not supported by Fluent NHibernate.");
        }

        public override void Visit(ComponentMappingBase componentMapping)
        {
            XmlDocument componentXml;
            var componentWriter = (componentMapping is ComponentMapping) ? new XmlComponentWriter(propertyWriter, parentWriter, versionWriter, oneToOneWriter) : null;
            var dynamicComponentWriter = (componentMapping is DynamicComponentMapping) ? new XmlDynamicComponentWriter(propertyWriter, parentWriter, versionWriter, oneToOneWriter) : null;

            if (componentWriter != null)
                componentXml = componentWriter.Write((ComponentMapping)componentMapping);
            else if (dynamicComponentWriter != null)
                componentXml = dynamicComponentWriter.Write((DynamicComponentMapping)componentMapping);
            else
                throw new ArgumentException("Not of ComponentMapping or DynamicComponentMapping type", "componentMapping");

            document.ImportAndAppendChild(componentXml);
        }

        public override void Visit(ParentMapping parentMapping)
        {
            var parentXml = parentWriter.Write(parentMapping);

            document.ImportAndAppendChild(parentXml);
        }
    }
}
