using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

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

            if (mapping.IsSpecified("Access"))
                element.WithAtt("access", mapping.Access);

            if (mapping.IsSpecified("Cascade"))
                element.WithAtt("cascade", mapping.Cascade);

            if (mapping.IsSpecified("Class"))
                element.WithAtt("class", mapping.Class);

            if (mapping.IsSpecified("Constrained"))
                element.WithAtt("constrained", mapping.Constrained);

            if (mapping.IsSpecified("Fetch"))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.IsSpecified("ForeignKey"))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.IsSpecified("Lazy"))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.IsSpecified("Name"))
                element.WithAtt("name", mapping.Name);

            if (mapping.IsSpecified("PropertyRef"))
                element.WithAtt("property-ref", mapping.PropertyRef);

            if (mapping.IsSpecified("EntityName"))
                element.WithAtt("entity-name", mapping.EntityName);
        }
    }
}