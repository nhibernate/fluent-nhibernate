using FluentNHibernate.Conventions.InspectionDsl;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public interface IExpectation
    {
        bool Matches(IInspector inspector);
    }
}