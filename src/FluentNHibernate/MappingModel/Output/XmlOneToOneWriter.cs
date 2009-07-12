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

            if (mapping.IsSpecified(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.IsSpecified(x => x.Cascade))
                element.WithAtt("cascade", mapping.Cascade);

            if (mapping.IsSpecified(x => x.Class))
                element.WithAtt("class", mapping.Class);

            if (mapping.IsSpecified(x => x.Constrained))
                element.WithAtt("constrained", mapping.Constrained);

            if (mapping.IsSpecified(x => x.Fetch))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.IsSpecified(x => x.ForeignKey))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.IsSpecified(x => x.Lazy))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.IsSpecified(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.IsSpecified(x => x.OuterJoin))
                element.WithAtt("outer-join", mapping.OuterJoin);

            if (mapping.IsSpecified(x => x.PropertyRef))
                element.WithAtt("property-ref", mapping.PropertyRef);
        }
    }
}