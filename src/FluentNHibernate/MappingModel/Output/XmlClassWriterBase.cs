using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class XmlClassWriterBase : NullMappingModelVisitor
    {
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        private readonly IXmlWriter<VersionMapping> versionWriter;
        private readonly IXmlWriter<OneToOneMapping> oneToOneWriter;
        protected XmlDocument document;

        protected XmlClassWriterBase(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<VersionMapping> versionWriter, IXmlWriter<OneToOneMapping> oneToOneWriter)
        {
            this.propertyWriter = propertyWriter;
            this.versionWriter = versionWriter;
            this.oneToOneWriter = oneToOneWriter;
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

        public override void Visit(OneToOneMapping mapping)
        {
            var oneToOneXml = oneToOneWriter.Write(mapping);

            document.ImportAndAppendChild(oneToOneXml);
        }
    }
}