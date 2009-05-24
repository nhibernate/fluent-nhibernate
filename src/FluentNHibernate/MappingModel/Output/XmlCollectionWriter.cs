using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCollectionWriter : NullMappingModelVisitor, IXmlWriter<ICollectionMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(ICollectionMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }
    }
}