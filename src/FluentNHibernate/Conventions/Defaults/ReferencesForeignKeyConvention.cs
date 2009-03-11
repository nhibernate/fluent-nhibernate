using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class ReferencesForeignKeyConvention : IReferenceConvention
    {
        public bool Accept(IManyToOnePart target)
        {
            return string.IsNullOrEmpty(target.ColumnName);
        }

        public void Apply(IManyToOnePart target, ConventionOverrides overrides)
        {
            if (overrides.GetForeignKeyName == null)
                target.TheColumnNameIs(target.Property.Name + "_id");
            else
                target.TheColumnNameIs(overrides.GetForeignKeyName(target.Property));
        }
    }
}