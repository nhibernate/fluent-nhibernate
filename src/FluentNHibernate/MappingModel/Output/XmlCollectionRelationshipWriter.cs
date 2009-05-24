using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCollectionRelationshipWriter : NullMappingModelVisitor, IXmlWriter<ICollectionRelationshipMapping>
    {
        private readonly IXmlWriter<OneToManyMapping> oneToManyWriter;
        private readonly IXmlWriter<ManyToManyMapping> manyToManyWriter;
        private XmlDocument document;

        public XmlCollectionRelationshipWriter(IXmlWriter<OneToManyMapping> oneToManyWriter, IXmlWriter<ManyToManyMapping> manyToManyWriter)
        {
            this.oneToManyWriter = oneToManyWriter;
            this.manyToManyWriter = manyToManyWriter;
        }

        public XmlDocument Write(ICollectionRelationshipMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessOneToMany(OneToManyMapping mapping)
        {
            document = oneToManyWriter.Write(mapping);
        }

        public override void ProcessManyToMany(ManyToManyMapping mapping)
        {
            document = manyToManyWriter.Write(mapping);
        }
    }
}