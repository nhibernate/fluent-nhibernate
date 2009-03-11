using System;
using System.Xml;

namespace FluentNHibernate
{
    public interface IMappingVisitor
    {
        Type CurrentType { get; set; }
        void AddMappingDocument(XmlDocument document, Type type);
    }
}
