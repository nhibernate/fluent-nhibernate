using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class XmlClassWriterBase : NullMappingModelVisitor
    {
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        private readonly IXmlWriter<VersionMapping> versionWriter;
        protected XmlDocument document;

        protected XmlClassWriterBase(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<VersionMapping> versionWriter)
        {
            this.propertyWriter = propertyWriter;
            this.versionWriter = versionWriter;
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            var propertyXml = propertyWriter.Write(propertyMapping);

            document.ImportAndAppendChild(propertyXml);
        }

        public override void Visit(VersionMapping versionMapping)
        {
            var versionXml = versionWriter.Write(versionMapping);

            document.ImportAndAppendChild(versionXml);
        }
    }
}