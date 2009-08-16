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
        private readonly IList<IAcceptanceCriteria<TInspector>> subCriteria;

        public AnyExpectation(IEnumerable<IAcceptanceCriteria<TInspector>> subCriteria)
        {
            this.subCriteria = new List<IAcceptanceCriteria<TInspector>>(subCriteria);
        }

        public bool Matches(IInspector inspector)
        {
            return subCriteria.Any(e => e.Matches(inspector));
        }

        bool IExpectation.Matches(IInspector inspector)
        {
            if (inspector is TInspector)
                return Matches((TInspector)inspector);

            return false;
        }
    }
}
