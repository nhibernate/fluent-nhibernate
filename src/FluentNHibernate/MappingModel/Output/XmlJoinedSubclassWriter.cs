using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlJoinedSubclassWriter : XmlClassWriterBase, IXmlWriter<JoinedSubclassMapping>
    {
        private readonly IXmlWriter<KeyMapping> keyWriter;
        private readonly IXmlWriter<DynamicComponentMapping> dynamicComponentMapping;

        public XmlJoinedSubclassWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<KeyMapping> keyWriter, IXmlWriter<DynamicComponentMapping> dynamicComponentMapping)
            : base(propertyWriter)
        {
            this.keyWriter = keyWriter;
            this.dynamicComponentMapping = dynamicComponentMapping;
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

        public override void Visit(DynamicComponentMapping componentMapping)
        {
            var dynamicComponentXml = dynamicComponentMapping.Write(componentMapping);

            document.ImportAndAppendChild(dynamicComponentXml);
        }
    }
}