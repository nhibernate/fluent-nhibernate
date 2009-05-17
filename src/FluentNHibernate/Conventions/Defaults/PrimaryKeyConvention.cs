using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
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
            acceptance.Expect(x => x.ColumnName, Is.Not.Set);
        }

        public void Apply(IIdentityAlteration alteration, IIdentityInspector inspector)
        {
            alteration.ColumnName(inspector.Property.Name);
        }
    }
}