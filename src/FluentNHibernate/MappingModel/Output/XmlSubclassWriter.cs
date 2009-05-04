using System.Xml;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlSubclassWriter : NullMappingModelVisitor, IXmlWriter<SubclassMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(SubclassMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            document = new XmlDocument();
        }
    }
}