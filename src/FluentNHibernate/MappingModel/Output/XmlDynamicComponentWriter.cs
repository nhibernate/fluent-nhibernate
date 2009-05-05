using System.Xml;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlDynamicComponentWriter : XmlComponentWriterBase<DynamicComponentMapping>, IXmlWriter<DynamicComponentMapping>
    {
        public XmlDynamicComponentWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ParentMapping> parentWriter)
            : base(propertyWriter, parentWriter)
        {
        }
        
        public XmlDocument Write(DynamicComponentMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessDynamicComponent(DynamicComponentMapping componentMapping)
        {
            ProcessComponentBase(componentMapping);
        }
    }
}