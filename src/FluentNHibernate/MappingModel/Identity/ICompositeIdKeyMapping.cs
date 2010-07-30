using System.Collections.Generic;

namespace FluentNHibernate.MappingModel.Identity
{
    public interface ICompositeIdKeyMapping
    {
        IEnumerable<ColumnMapping> Columns { get; }
        string Name { get; }
        string Access { get; }
    }
}