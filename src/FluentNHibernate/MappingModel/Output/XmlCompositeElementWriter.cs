using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCompositeElementWriter : NullMappingModelVisitor, IXmlWriter<CompositeElementMapping>
    {
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        protected XmlDocument document;

        public XmlCompositeElementWriter(IXmlWriter<PropertyMapping> propertyWriter)
        {
            this.propertyWriter = propertyWriter;
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            var propertyXml = propertyWriter.Write(propertyMapping);

            document.ImportAndAppendChild(propertyXml);
        }

        //TODO: Implement Visit for ManyToOneMapping when writer is available

        public XmlDocument Write(CompositeElementMapping compositeElement)
        {
            document = null;
            compositeElement.AcceptVisitor(this);
            return document;
        }

        public override void ProcessCompositeElement(CompositeElementMapping compositeElementMapping)
        {
            document = new XmlDocument();
            var element = document.CreateElement("composite-element");

            var typeName = compositeElementMapping.Type != null ? compositeElementMapping.Type.AssemblyQualifiedName : string.Empty;
            element.WithAtt("class", typeName);

            foreach (var attribute in compositeElementMapping.UnmigratedAttributes)
                element.WithAtt(attribute.Key, attribute.Value);

            var sortedUnmigratedParts = new List<IMappingPart>(compositeElementMapping.UnmigratedParts);
            sortedUnmigratedParts.Sort(new MappingPartComparer(compositeElementMapping.UnmigratedParts));
            foreach (var part in sortedUnmigratedParts)
            {
                part.Write(element, null);
            }
            foreach (var attribute in compositeElementMapping.UnmigratedAttributes)
            {
                element.WithAtt(attribute.Key, attribute.Value);
            }

            document.AppendChild(element);
        }
    }
}
