using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class VersionColumnNameConvention : IVersionConvention
    {
        public bool Accept(IVersion target)
        {
            return string.IsNullOrEmpty(target.ColumnName);
        }

        public void Apply(IVersion target, ConventionOverrides overrides)
        {
            if (overrides.GetVersionColumnName == null)
                target.TheColumnNameIs(target.Property.Name);
            else
                target.TheColumnNameIs(overrides.GetVersionColumnName(target.Property));
        }
    }
}