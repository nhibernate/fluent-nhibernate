using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlManyToOneWriter : NullMappingModelVisitor, IXmlWriter<ManyToOneMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(ManyToOneMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessManyToOne(ManyToOneMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("many-to-one");
        }
    }
}