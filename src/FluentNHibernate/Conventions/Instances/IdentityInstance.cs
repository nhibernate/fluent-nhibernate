using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class IdentityInstance : IdentityInspector, IIdentityInstance
    {
        private readonly IdMapping mapping;

        public IdentityInstance(IdMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new void ColumnName(string columnName)
        {
            var columnAttributes = mapping.Columns.First().Attributes.Clone();

            mapping.ClearColumns();
            mapping.AddColumn(new ColumnMapping(columnAttributes) { Name = columnName });
        }
    }
}