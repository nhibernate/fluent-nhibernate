using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlStoredProcedureWriter : XmlClassWriterBase, IXmlWriter<StoredProcedureMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;

        public XmlStoredProcedureWriter(IXmlWriterServiceLocator serviceLocator) : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(StoredProcedureMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessStoredProcedure(StoredProcedureMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement(mapping.SPType);
            element.WithAtt("check", mapping.Check);
            element.InnerXml = mapping.Query;

        }

        public override void Visit(StoredProcedureMapping mapping)
        {
            var writer = serviceLocator.GetWriter<StoredProcedureMapping>();
            var xml = writer.Write(mapping);

            document.ImportAndAppendChild(xml);
        }
    }
}
