using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlClassWriter : NullMappingModelVisitor, IXmlWriter<ClassMapping>
    {
        private XmlDocument document;
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        private readonly IXmlWriter<DiscriminatorMapping> discriminatorWriter;
        private readonly IXmlWriter<ISubclassMapping> subclassWriter;

        public XmlClassWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<DiscriminatorMapping> discriminatorWriter, IXmlWriter<ISubclassMapping> subclassWriter)
        {
            this.propertyWriter = propertyWriter;
            this.discriminatorWriter = discriminatorWriter;
            this.subclassWriter = subclassWriter;
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
                .WithAtt("table", classMapping.Tablename)
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2");

            if (classMapping.BatchSize > 0)
                classElement.WithAtt("batch-size", classMapping.BatchSize.ToString());

            return classElement;
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            var propertyXml = propertyWriter.Write(propertyMapping);

            document.ImportAndAppendChild(propertyXml);
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
    }
}
