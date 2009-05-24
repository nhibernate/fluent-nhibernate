using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlClassWriter : XmlClassWriterBase, IXmlWriter<ClassMapping>
    {
        private readonly IXmlWriter<DiscriminatorMapping> discriminatorWriter;
        private readonly IXmlWriter<ISubclassMapping> subclassWriter;
        private readonly IXmlWriter<ComponentMapping> componentWriter;
        private readonly IXmlWriter<DynamicComponentMapping> dynamicComponentWriter;
        private readonly IXmlWriter<JoinMapping> joinWriter;
        private readonly IXmlWriter<IIdentityMapping> idWriter;

        public XmlClassWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<DiscriminatorMapping> discriminatorWriter, IXmlWriter<ISubclassMapping> subclassWriter, IXmlWriter<ComponentMapping> componentWriter, IXmlWriter<DynamicComponentMapping> dynamicComponentWriter, IXmlWriter<JoinMapping> joinWriter, IXmlWriter<VersionMapping> versionWriter, IXmlWriter<IIdentityMapping> idWriter)
            : base(propertyWriter, versionWriter)
        {
            this.discriminatorWriter = discriminatorWriter;
            this.joinWriter = joinWriter;
            this.idWriter = idWriter;
            this.subclassWriter = subclassWriter;
            this.componentWriter = componentWriter;
            this.dynamicComponentWriter = dynamicComponentWriter;
        }

        public XmlDocument Write(ClassMapping mapping)
        {
            document = null;
            mapping.AcceptVisitor(this);
            return document;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            document = new XmlDocument();

            // create class node
            var classElement = CreateClassElement(classMapping);
            var sortedUnmigratedParts = new List<IMappingPart>(classMapping.UnmigratedParts);

            sortedUnmigratedParts.Sort(new MappingPartComparer(classMapping.UnmigratedParts));

            foreach (var part in sortedUnmigratedParts)
            {
                part.Write(classElement, null);
            }

            foreach (var attribute in classMapping.UnmigratedAttributes)
            {
                classElement.WithAtt(attribute.Key, attribute.Value);
            }
        }

        protected virtual XmlElement CreateClassElement(ClassMapping classMapping)
        {
            var typeName = classMapping.Type != null ? classMapping.Type.AssemblyQualifiedName : string.Empty;

            var classElement = document.CreateElement("class");

            document.AppendChild(classElement);

            classElement.WithAtt("name", typeName)
                .WithAtt("table", classMapping.TableName)
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2");

            if (classMapping.Attributes.IsSpecified(x => x.BatchSize))
                classElement.WithAtt("batch-size", classMapping.BatchSize);

            if (classMapping.Attributes.IsSpecified(x => x.DiscriminatorBaseValue))
                classElement.WithAtt("discriminator-value", classMapping.DiscriminatorBaseValue.ToString());

            if (classMapping.Attributes.IsSpecified(x => x.DynamicInsert))
                classElement.WithAtt("dynamic-insert", classMapping.DynamicInsert);

            if (classMapping.Attributes.IsSpecified(x => x.DynamicUpdate))
                classElement.WithAtt("dynamic-update", classMapping.DynamicUpdate);

            if (classMapping.Attributes.IsSpecified(x => x.LazyLoad))
                classElement.WithAtt("lazy", classMapping.LazyLoad);

            if (classMapping.Attributes.IsSpecified(x => x.Schema))
                classElement.WithAtt("schema", classMapping.Schema);

            if (classMapping.Attributes.IsSpecified(x => x.Mutable))
                classElement.WithAtt("mutable", classMapping.Mutable);

            return classElement;
        }

        public override void Visit(DiscriminatorMapping discriminatorMapping)
        {
            var discriminatorXml = discriminatorWriter.Write(discriminatorMapping);

            document.ImportAndAppendChild(discriminatorXml);
        }

        public override void Visit(ISubclassMapping subclassMapping)
        {
            var subclassXml = subclassWriter.Write(subclassMapping);

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

        public override void Visit(JoinMapping joinMapping)
        {
            var joinXml = joinWriter.Write(joinMapping);

            document.ImportAndAppendChild(joinXml);
        }

        public override void Visit(IIdentityMapping mapping)
        {
            var idXml = idWriter.Write(mapping);

            document.ImportAndAppendChild(idXml);
        }
    }
}
