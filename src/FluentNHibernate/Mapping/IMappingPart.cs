using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface IMappingPart : IHasAttributes
    {
        void Write(XmlElement classElement, IMappingVisitor visitor);
        int Level { get; }
    }
}