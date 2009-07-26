using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class AnyExpectation<TInspector> : IExpectation
        where TInspector : IInspector
    {
        private readonly IList<IExpectation> expectations;

        public AnyExpectation(IEnumerable<IExpectation> expectations)
        {
            this.expectations = new List<IExpectation>(expectations);
        }

        public bool Matches(IInspector inspector)
        {
            return expectations.Any(e => e.Matches(inspector));
        }

        bool IExpectation.Matches(IInspector inspector)
        {
            if (inspector is TInspector)
                return Matches((TInspector)inspector);

            return false;
        }
    }
}
