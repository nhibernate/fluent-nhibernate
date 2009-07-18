using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public interface IUserTypeConvention : IConventionAcceptance<IPropertyInspector>, IPropertyConvention
    {
    }
}