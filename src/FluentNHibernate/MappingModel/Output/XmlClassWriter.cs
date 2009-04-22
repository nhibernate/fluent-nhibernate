using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlClassWriter : NullMappingModelVisitor, IXmlWriter<ClassMapping>
    {
        private XmlDocument document;
        private readonly IXmlWriter<PropertyMapping> propertyWriter;

        public XmlClassWriter(IXmlWriter<PropertyMapping> propertyWriter)
        {
            this.propertyWriter = propertyWriter;
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

            sortedUnmigratedParts.Sort(new MappingPartComparer());

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
            var propertyNode = document.ImportNode(propertyXml.DocumentElement, true);

            document.DocumentElement.AppendChild(propertyNode);
        }
    }
}
