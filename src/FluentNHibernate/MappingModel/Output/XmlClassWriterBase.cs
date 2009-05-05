using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class XmlClassWriterBase : NullMappingModelVisitor
    {
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        protected XmlDocument document;

        protected XmlClassWriterBase(IXmlWriter<PropertyMapping> propertyWriter)
        {
            this.propertyWriter = propertyWriter;
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            var propertyXml = propertyWriter.Write(propertyMapping);

            document.ImportAndAppendChild(propertyXml);
        }
    }
}