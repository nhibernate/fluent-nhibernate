using System;
using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlInheritanceWriter : NullMappingModelVisitor, IXmlWriter<ISubclassMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlInheritanceWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(ISubclassMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            var writer = serviceLocator.GetWriter<SubclassMapping>();
            document = writer.Write(subclassMapping);
        }

        public override void ProcessJoinedSubclass(JoinedSubclassMapping subclassMapping)
        {
            var writer = serviceLocator.GetWriter<JoinedSubclassMapping>();
            document = writer.Write(subclassMapping);
        }
    }
}