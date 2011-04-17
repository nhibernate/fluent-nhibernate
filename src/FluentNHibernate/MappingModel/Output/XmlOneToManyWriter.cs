using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlOneToManyWriter : NullMappingModelVisitor, IXmlWriter<OneToManyMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(OneToManyMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessOneToMany(OneToManyMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("one-to-many");

            if (mapping.IsSpecified("Class"))
                element.WithAtt("class", mapping.Class);

            if (mapping.IsSpecified("NotFound"))
                element.WithAtt("not-found", mapping.NotFound);

            if (mapping.IsSpecified("EntityName"))
                element.WithAtt("entity-name", mapping.EntityName);
        }
    }
}