using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlSubclassWriter : XmlClassWriterBase, IXmlWriter<SubclassMapping>
    {
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        private readonly IXmlWriter<ComponentMapping> componentWriter;
        private readonly IXmlWriter<DynamicComponentMapping> dynamicComponentWriter;
        private readonly IXmlWriter<VersionMapping> versionWriter;

        public XmlSubclassWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ComponentMapping> componentWriter, IXmlWriter<DynamicComponentMapping> dynamicComponentWriter, IXmlWriter<VersionMapping> versionWriter)
            : base(propertyWriter, versionWriter)
        {
            this.propertyWriter = propertyWriter;
            this.componentWriter = componentWriter;
            this.dynamicComponentWriter = dynamicComponentWriter;
            this.versionWriter = versionWriter;
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

            if (subclassMapping.Attributes.IsSpecified(x => x.Proxy))
                subclassElement.WithAtt("proxy", subclassMapping.Proxy.AssemblyQualifiedName);

            if (subclassMapping.Attributes.IsSpecified(x => x.Lazy))
                subclassElement.WithAtt("lazy", subclassMapping.Lazy);

            if (subclassMapping.Attributes.IsSpecified(x => x.DynamicUpdate))
                subclassElement.WithAtt("dynamic-update", subclassMapping.DynamicUpdate);

            if (subclassMapping.Attributes.IsSpecified(x => x.DynamicInsert))
                subclassElement.WithAtt("dynamic-insert", subclassMapping.DynamicInsert);

            if (subclassMapping.Attributes.IsSpecified(x => x.SelectBeforeUpdate))
                subclassElement.WithAtt("select-before-update", subclassMapping.SelectBeforeUpdate);

            if (subclassMapping.Attributes.IsSpecified(x => x.Abstract))
                subclassElement.WithAtt("abstract", subclassMapping.Abstract);

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
            var subWriter = new XmlSubclassWriter(propertyWriter, componentWriter, dynamicComponentWriter, versionWriter);
            var subclassXml = subWriter.Write(subclassMapping);

            document.ImportAndAppendChild(subclassXml);
        }

        public override void Visit(ComponentMappingBase componentMapping)
        {
            var dynamicComponentMapping = componentMapping as DynamicComponentMapping;
            XmlDocument componentXml = (dynamicComponentMapping != null) 
                                        ? dynamicComponentWriter.Write(dynamicComponentMapping) 
                                        : componentWriter.Write((ComponentMapping)componentMapping);

            document.ImportAndAppendChild(componentXml);
        }
    }
}