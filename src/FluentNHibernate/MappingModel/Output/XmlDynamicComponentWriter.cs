using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlDynamicComponentWriter : XmlComponentWriterBase<DynamicComponentMapping>, IXmlWriter<DynamicComponentMapping>
    {
        public XmlDynamicComponentWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ParentMapping> parentWriter, IXmlWriter<VersionMapping> versionWriter, IXmlWriter<OneToOneMapping> oneToOneWriter)
            : base(propertyWriter, parentWriter, versionWriter, oneToOneWriter)
        {
        }

        public XmlDocument Write(DynamicComponentMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }
    }
}