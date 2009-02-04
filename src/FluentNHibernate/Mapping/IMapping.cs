using System.Collections.Generic;

namespace FluentNHibernate.Mapping
{
    public interface IMapping
    {
        IList<IMappingPart> Parts { get; }
        void ApplyMappings(IMappingVisitor visitor);
    }
}