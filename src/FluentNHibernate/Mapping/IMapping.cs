using System.Xml;

namespace ShadeTree.DomainModel.Mapping
{
    public interface IMapping
    {
        void ApplyMappings(IMappingVisitor visitor);
    }

    public interface IMappingPart
    {
        void Write(XmlElement classElement, IMappingVisitor visitor);
        int Level { get; }
    }
}