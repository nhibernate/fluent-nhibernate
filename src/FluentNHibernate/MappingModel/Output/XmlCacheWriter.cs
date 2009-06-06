using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCacheWriter : NullMappingModelVisitor, IXmlWriter<CacheMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(CacheMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessCache(CacheMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("cache");

            if (mapping.Attributes.IsSpecified(x => x.Region))
                element.WithAtt("region", mapping.Region);

            if (mapping.Attributes.IsSpecified(x => x.Usage))
                element.WithAtt("usage", mapping.Usage);
        }
    }
}