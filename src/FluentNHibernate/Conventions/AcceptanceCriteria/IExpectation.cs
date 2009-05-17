using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public interface IExpectation
    {
        bool Matches(IInspector inspector);
    }
}