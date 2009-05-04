using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlSubclassWriter : XmlClasslikeWriterBase, IXmlWriter<SubclassMapping>
    {
        private readonly IXmlWriter<PropertyMapping> propertyWriter;

        public XmlSubclassWriter(IXmlWriter<PropertyMapping> propertyWriter)
            : base(propertyWriter)
        {
            this.propertyWriter = propertyWriter;
        }

        public XmlDocument Write(SubclassMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            document = new XmlDocument();

            var subclassElement = document
                .AddElement("subclass")
                .WithAtt("name", subclassMapping.Type.AssemblyQualifiedName);

            if (subclassMapping.Attributes.IsSpecified(x => x.DiscriminatorValue))
                subclassElement.WithAtt("discriminator-value", subclassMapping.DiscriminatorValue.ToString());

            if (subclassMapping.Attributes.IsSpecified(x => x.LazyLoad))
                subclassElement.WithAtt("lazy", subclassMapping.LazyLoad);

            var sortedUnmigratedParts = new List<IMappingPart>(subclassMapping.UnmigratedParts);

            sortedUnmigratedParts.Sort(new MappingPartComparer(subclassMapping.UnmigratedParts));

            foreach (var part in sortedUnmigratedParts)
            {
                part.Write(subclassElement, null);
            }

            foreach (var attribute in subclassMapping.UnmigratedAttributes)
            {
                subclassElement.WithAtt(attribute.Key, attribute.Value);
            }
        }

        public override void Visit(SubclassMapping subclassMapping)
        {
            var subWriter = new XmlSubclassWriter(propertyWriter);
            var subclassXml = subWriter.Write(subclassMapping);

            document.ImportAndAppendChild(subclassXml);
        }
    }
}