using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlJoinedSubclassWriter : XmlClasslikeWriterBase, IXmlWriter<JoinedSubclassMapping>
    {
        private readonly IXmlWriter<KeyMapping> keyWriter;

        public XmlJoinedSubclassWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<KeyMapping> keyWriter)
            : base(propertyWriter)
        {
            this.keyWriter = keyWriter;
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
    }
}