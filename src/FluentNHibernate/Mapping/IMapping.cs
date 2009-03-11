using System.Collections.Generic;

namespace FluentNHibernate.Mapping
{
    public interface IMapping
    {
        IEnumerable<IMappingPart> Parts { get; }
        void ApplyMappings(IMappingVisitor visitor);
    }
}