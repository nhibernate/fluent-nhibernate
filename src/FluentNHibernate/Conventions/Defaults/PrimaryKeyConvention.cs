using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.InspectionDsl;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default primary key name convention.
    /// </summary>
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Accept(IAcceptanceCriteria<IIdentityInspector> acceptance)
        {
            acceptance.Expect(x => x.ColumnName, Is.Not.Set);
        }

        public void Apply(IIdentityInspector target)
        {
            //target.ColumnName(target.Property.Name);
        }
    }
}