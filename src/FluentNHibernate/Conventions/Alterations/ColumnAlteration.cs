using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Alterations
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