using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCollectionRelationshipWriter : NullMappingModelVisitor, IXmlWriter<ICollectionRelationshipMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlCollectionRelationshipWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(ICollectionRelationshipMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessOneToMany(OneToManyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<OneToManyMapping>();
            document = writer.Write(mapping);
        }

        public override void ProcessManyToMany(ManyToManyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ManyToManyMapping>();
            document = writer.Write(mapping);
        }
    }
}