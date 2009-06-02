using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class BaseXmlComponentWriter : XmlClassWriterBase
    {
        private readonly IXmlWriterServiceLocator serviceLocator;

        protected BaseXmlComponentWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        protected XmlDocument WriteComponent(string element, ComponentMappingBase mapping)
        {
            var doc = new XmlDocument();
            var componentElement = doc.AddElement(element);

            if (mapping.Attributes.IsSpecified(x => x.Name))
                componentElement.WithAtt("name", mapping.Name);

            if (mapping.Attributes.IsSpecified(x => x.Insert))
                componentElement.WithAtt("insert", mapping.Insert);

            if (mapping.Attributes.IsSpecified(x => x.Update))
                componentElement.WithAtt("update", mapping.Update);

            var sortedUnmigratedParts = new List<IMappingPart>(mapping.UnmigratedParts);

            sortedUnmigratedParts.Sort(new MappingPartComparer(mapping.UnmigratedParts));

            foreach (var part in sortedUnmigratedParts)
            {
                part.Write(componentElement, null);
            }

            foreach (var attribute in mapping.UnmigratedAttributes)
            {
                componentElement.WithAtt(attribute.Key, attribute.Value);
            }

            return doc;
        }

        public override void Visit(ComponentMappingBase componentMapping)
        {
            var writer = serviceLocator.GetWriter<ComponentMappingBase>();
            var componentXml = writer.Write(componentMapping);

            document.ImportAndAppendChild(componentXml);
        }

        public override void Visit(ParentMapping parentMapping)
        {
            var writer = serviceLocator.GetWriter<ParentMapping>();
            var parentXml = writer.Write(parentMapping);

            document.ImportAndAppendChild(parentXml);
        }
    }
}
