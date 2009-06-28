using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default primary key name convention.
    /// </summary>
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Accept(IAcceptanceCriteria<IIdentityInspector> acceptance)
        {
            acceptance.Expect(x => x.Columns.IsEmpty());
        }

        public void Apply(IIdentityInstance instance)
        {
            instance.ColumnName(instance.Property.Name);
        }
    }
}