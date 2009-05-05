using System.Xml;

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

        public override void ProcessComponent(ComponentMapping componentMapping)
        {
            ProcessComponentBase(componentMapping);
        }
    }
}
