using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public bool Accept(IIdentityPart target)
        {
            return string.IsNullOrEmpty(target.ColumnName);
        }

        public void Apply(IIdentityPart target)
        {
            target.TheColumnNameIs(target.Property.Name);
        }
    }
}