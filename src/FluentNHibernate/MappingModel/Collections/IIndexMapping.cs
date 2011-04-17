using System.Collections.Generic;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface IIndexMapping
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
        IEnumerable<ColumnMapping> Columns { get; }
    }
}