using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class InvertedExpectation(IExpectation expectation) : IExpectation
{
    public bool Matches(IInspector inspector)
    {
        return !expectation.Matches(inspector);
    }
}
