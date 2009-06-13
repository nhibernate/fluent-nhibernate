using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class ManyToOneAlteration : IManyToOneAlteration
    {
        private readonly ManyToOneMapping mapping;

        public ManyToOneAlteration(ManyToOneMapping mapping)
        {
            this.mapping = mapping;
        }

        public void ColumnName(string columnName)
        {
            mapping.AddColumn(new ColumnMapping { Name = columnName });
        }
    }
}