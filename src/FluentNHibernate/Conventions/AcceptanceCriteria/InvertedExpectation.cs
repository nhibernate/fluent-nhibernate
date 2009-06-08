using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class InvertedExpectation : IExpectation
    {
        private readonly IExpectation expectation;

        public InvertedExpectation(IExpectation expectation)
        {
            this.expectation = expectation;
        }

        public bool Matches(IInspector inspector)
        {
            return !expectation.Matches(inspector);
        }
    }
}