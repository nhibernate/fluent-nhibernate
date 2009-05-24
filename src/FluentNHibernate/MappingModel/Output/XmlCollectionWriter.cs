using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCollectionWriter : NullMappingModelVisitor, IXmlWriter<ICollectionMapping>
    {
        private readonly IXmlWriter<BagMapping> bagWriter;
        private readonly IXmlWriter<SetMapping> setWriter;
        private XmlDocument document;

        public XmlCollectionWriter(IXmlWriter<BagMapping> bagWriter, IXmlWriter<SetMapping> setWriter)
        {
            this.bagWriter = bagWriter;
            this.setWriter = setWriter;
        }

        public XmlDocument Write(ICollectionMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessBag(BagMapping mapping)
        {
            document = bagWriter.Write(mapping);
        }

        public override void ProcessSet(SetMapping mapping)
        {
            document = setWriter.Write(mapping);
        }
    }
}