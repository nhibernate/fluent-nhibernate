using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlClassWriter : XmlClasslikeWriterBase, IXmlWriter<ClassMapping>
    {
        private readonly IXmlWriter<DiscriminatorMapping> discriminatorWriter;
        private readonly IXmlWriter<ISubclassMapping> subclassWriter;
        private readonly IXmlWriter<DynamicComponentMapping> dynamicComponentWriter;

        public XmlClassWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<DiscriminatorMapping> discriminatorWriter, IXmlWriter<ISubclassMapping> subclassWriter, IXmlWriter<DynamicComponentMapping> dynamicComponentWriter)
            : base(propertyWriter)
        {
            this.discriminatorWriter = discriminatorWriter;
            this.subclassWriter = subclassWriter;
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
            var typeName = classMapping.Type.IsGenericType ? classMapping.Type.AssemblyQualifiedName : classMapping.Type.AssemblyQualifiedName;

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

        public override void Visit(DynamicComponentMapping componentMapping)
        {
            var dynamicComponentXml = dynamicComponentWriter.Write(componentMapping);

            document.ImportAndAppendChild(dynamicComponentXml);
        }
    }
}
