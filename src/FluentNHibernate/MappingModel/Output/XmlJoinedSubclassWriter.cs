using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlJoinedSubclassWriter : XmlClassWriterBase, IXmlWriter<JoinedSubclassMapping>
    {
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        private readonly IXmlWriter<KeyMapping> keyWriter;
        private readonly IXmlWriter<ComponentMapping> componentWriter;
        private readonly IXmlWriter<DynamicComponentMapping> dynamicComponentWriter;
        private readonly IXmlWriter<VersionMapping> versionWriter;

        public XmlJoinedSubclassWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<KeyMapping> keyWriter, IXmlWriter<ComponentMapping> componentWriter, IXmlWriter<DynamicComponentMapping> dynamicComponentWriter, IXmlWriter<VersionMapping> versionWriter)
            : base(propertyWriter, versionWriter)
        {
            this.propertyWriter = propertyWriter;
            this.keyWriter = keyWriter;
            this.componentWriter = componentWriter;
            this.dynamicComponentWriter = dynamicComponentWriter;
            this.versionWriter = versionWriter;
        }

        public XmlDocument Write(JoinedSubclassMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {
            document = new XmlDocument();

            var subclassElement = document.AddElement("joined-subclass")
                .WithAtt("name", subclassMapping.Name);

            if (subclassMapping.Attributes.IsSpecified(x => x.TableName))
                subclassElement.WithAtt("table", subclassMapping.TableName);

            if (subclassMapping.Attributes.IsSpecified(x => x.Schema))
                subclassElement.WithAtt("schema", subclassMapping.Schema);

            if (subclassMapping.Attributes.IsSpecified(x => x.Check))
                subclassElement.WithAtt("check", subclassMapping.Check);

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

        public override void Visit(KeyMapping keyMapping)
        {
            var keyXml = keyWriter.Write(keyMapping);

            document.ImportAndAppendChild(keyXml);
        }

        public override void Visit(ComponentMappingBase componentMapping)
        {
            var dynamicComponentMapping = componentMapping as DynamicComponentMapping;
            XmlDocument componentXml = (dynamicComponentMapping != null)
                                        ? dynamicComponentWriter.Write(dynamicComponentMapping)
                                        : componentWriter.Write((ComponentMapping)componentMapping);

            document.ImportAndAppendChild(componentXml);
        }

        public override void Visit(JoinedSubclassMapping subclassMapping)
        {
            var subWriter = new XmlJoinedSubclassWriter(propertyWriter, keyWriter, componentWriter, dynamicComponentWriter, versionWriter);
            var subclassXml = subWriter.Write(subclassMapping);

            document.ImportAndAppendChild(subclassXml);
        }
    }
}