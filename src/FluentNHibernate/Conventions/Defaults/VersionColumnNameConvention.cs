using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Version column naming convention
    /// </summary>
    public class VersionColumnNameConvention : IVersionConvention
    {
        public bool Accept(IVersion target)
        {
            return string.IsNullOrEmpty(target.ColumnName);
        }

        public void Apply(IVersion target)
        {
            target.TheColumnNameIs(target.Property.Name);
        }
    }
}