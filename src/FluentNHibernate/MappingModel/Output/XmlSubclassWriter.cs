using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlSubclassWriter : XmlClassWriterBase, IXmlWriter<SubclassMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;

        public XmlSubclassWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
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

            var subclassElement = document.AddElement("subclass");

            if (subclassMapping.Attributes.IsSpecified(x => x.Name))
                subclassElement.WithAtt("name", subclassMapping.Type.AssemblyQualifiedName);

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
            var writer = serviceLocator.GetWriter<SubclassMapping>();
            var subclassXml = writer.Write(subclassMapping);

            document.ImportAndAppendChild(subclassXml);
        }

        public override void Visit(ComponentMappingBase componentMapping)
        {
            var writer = serviceLocator.GetWriter<ComponentMappingBase>();
            var componentXml = writer.Write(componentMapping);

            document.ImportAndAppendChild(componentXml);
        }
    }
}