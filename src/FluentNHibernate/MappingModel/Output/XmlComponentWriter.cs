using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlComponentWriter : XmlComponentWriterBase<ComponentMapping>, IXmlWriter<ComponentMapping>
    {
        public XmlComponentWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ParentMapping> parentWriter)
            : base(propertyWriter, parentWriter)
        {
        }

        public XmlDocument Write(ComponentMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }
    }
}
