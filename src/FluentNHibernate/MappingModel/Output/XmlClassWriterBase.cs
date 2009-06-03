using System.Xml;
using FluentNHibernate.MappingModel.Collections;
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
            var xml = writer.Write(propertyMapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(VersionMapping versionMapping)
        {
            var writer = serviceLocator.GetWriter<VersionMapping>();
            var xml = writer.Write(versionMapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(OneToOneMapping mapping)
        {
            var writer = serviceLocator.GetWriter<OneToOneMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(ManyToOneMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ManyToOneMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(AnyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<AnyMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(ICollectionMapping collectionMapping)
        {
            var writer = serviceLocator.GetWriter<ICollectionMapping>();
            var xml = writer.Write(collectionMapping);

            document.ImportAndAppendChild(xml);
        }
    }
}