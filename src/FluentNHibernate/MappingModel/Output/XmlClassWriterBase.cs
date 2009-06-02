using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class XmlClassWriterBase : NullMappingModelVisitor
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        protected XmlDocument document;

        protected XmlClassWriterBase(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            var writer = serviceLocator.GetWriter<PropertyMapping>();
            var propertyXml = writer.Write(propertyMapping);

            document.ImportAndAppendChild(propertyXml);
        }

        public override void Visit(VersionMapping versionMapping)
        {
            var writer = serviceLocator.GetWriter<VersionMapping>();
            var versionXml = writer.Write(versionMapping);

            document.ImportAndAppendChild(versionXml);
        }

        public override void Visit(OneToOneMapping mapping)
        {
            var writer = serviceLocator.GetWriter<OneToOneMapping>();
            var oneToOneXml = writer.Write(mapping);

            document.ImportAndAppendChild(oneToOneXml);
        }
    }
}