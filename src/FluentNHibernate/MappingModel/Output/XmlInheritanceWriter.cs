using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlInheritanceWriter : NullMappingModelVisitor, IXmlWriter<ISubclassMapping>
    {
        private readonly IXmlWriter<SubclassMapping> subclassWriter;
        private readonly IXmlWriter<JoinedSubclassMapping> joinedSubclassWriter;
        private XmlDocument document;

        public XmlInheritanceWriter(IXmlWriter<SubclassMapping> subclassWriter, IXmlWriter<JoinedSubclassMapping> joinedSubclassWriter)
        {
            this.subclassWriter = subclassWriter;
            this.joinedSubclassWriter = joinedSubclassWriter;
        }

        public XmlDocument Write(ISubclassMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            document = subclassWriter.Write(subclassMapping);
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {
            document = joinedSubclassWriter.Write(subclassMapping);
        }
    }
}