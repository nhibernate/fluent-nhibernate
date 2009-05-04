using System.Xml;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlInheritanceWriter : NullMappingModelVisitor, IXmlWriter<ISubclassMapping>
    {
        private readonly IXmlWriter<SubclassMapping> subclassWriter;
        private XmlDocument document;

        public XmlInheritanceWriter(IXmlWriter<SubclassMapping> subclassWriter)
        {
            this.subclassWriter = subclassWriter;
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
    }
}