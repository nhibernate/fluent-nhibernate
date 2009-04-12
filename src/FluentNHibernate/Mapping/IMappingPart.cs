using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface IMappingPart : IHasAttributes
    {
        void Write(XmlElement classElement, IMappingVisitor visitor);
        
        /// <summary>
        /// Indicates a constant, general position on the document the part should be written to
        /// </summary>
        PartPosition PositionOnDocument { get; }

        /// <summary>
        /// Indicates a constant sub-position within a similar grouping of positions the element will be written in
        /// </summary>
        int LevelWithinPosition { get; }
    }
}