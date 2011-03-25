using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

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

        public override void Visit(CollectionMapping collectionMapping)
        {
            var writer = serviceLocator.GetWriter<CollectionMapping>();
            var xml = writer.Write(collectionMapping);

            document.ImportAndAppendChild(xml);
        }

        public override void Visit(StoredProcedureMapping mapping)
        {
            var writer = serviceLocator.GetWriter<StoredProcedureMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }
    }
}