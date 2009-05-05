using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default primary key name convention.
    /// </summary>
    public class PrimaryKeyConvention : IIdConvention
    {
        public bool Accept(IIdentityPart target)
        {
            return string.IsNullOrEmpty(target.GetColumnName());
        }

        public void Apply(IIdentityPart target)
        {
            target.ColumnName(target.Property.Name);
        }
    }
}