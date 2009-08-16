using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlOneToOneWriter : NullMappingModelVisitor, IXmlWriter<OneToOneMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(OneToOneMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessOneToOne(OneToOneMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("one-to-one");

            if (mapping.HasValue(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.HasValue(x => x.Cascade))
                element.WithAtt("cascade", mapping.Cascade);

            if (mapping.HasValue(x => x.Class))
                element.WithAtt("class", mapping.Class);

            if (mapping.HasValue(x => x.Constrained))
                element.WithAtt("constrained", mapping.Constrained);

            if (mapping.HasValue(x => x.Fetch))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.HasValue(x => x.ForeignKey))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.HasValue(x => x.Lazy))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.HasValue(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.HasValue(x => x.PropertyRef))
                element.WithAtt("property-ref", mapping.PropertyRef);
        }
    }
}