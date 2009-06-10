using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class ColumnAlteration : IColumnAlteration
    {
        private readonly ColumnMapping mapping;

        public ColumnAlteration(ColumnMapping mapping)
        {
            this.mapping = mapping;
        }
    }
}