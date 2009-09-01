using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlCollectionWriter : NullMappingModelVisitor, IXmlWriter<ICollectionMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlCollectionWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(ICollectionMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessBag(BagMapping mapping)
        {
            var writer = serviceLocator.GetWriter<BagMapping>();
            document = writer.Write(mapping);
        }

        public override void ProcessSet(SetMapping mapping)
        {
            var writer = serviceLocator.GetWriter<SetMapping>();
            document = writer.Write(mapping);
        }

        public override void ProcessList(ListMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ListMapping>();
            document = writer.Write(mapping);
        }

        public override void ProcessMap(MapMapping mapping)
        {
            var writer = serviceLocator.GetWriter<MapMapping>();
            document = writer.Write(mapping);
        }

        public override void ProcessArray(ArrayMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ArrayMapping>();
            document = writer.Write(mapping);
        }

        public override void Visit(FilterMapping filterMapping)
        {
            var writer = serviceLocator.GetWriter<FilterMapping>();
            var xml = writer.Write(filterMapping);
            document.ImportAndAppendChild(xml);
        }
    }
}