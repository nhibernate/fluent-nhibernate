using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class AnyExpectation<TInspector>(IEnumerable<IAcceptanceCriteria<TInspector>> subCriteria) : IExpectation
    where TInspector : IInspector
{
    private readonly IList<IAcceptanceCriteria<TInspector>> subCriteria = new List<IAcceptanceCriteria<TInspector>>(subCriteria);

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
