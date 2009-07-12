using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public interface IUserTypeConvention
    {
        void Accept(IAcceptanceCriteria<IPropertyInspector> acceptance);
        void Apply(IPropertyInstance instance);
    }
}