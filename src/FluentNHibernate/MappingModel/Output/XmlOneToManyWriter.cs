using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

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

            if (mapping.HasValue(x => x.Class))
                element.WithAtt("class", mapping.Class);

            if (mapping.HasValue(x => x.NotFound))
                element.WithAtt("not-found", mapping.NotFound);
        }
    }
}