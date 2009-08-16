using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public interface IAcceptanceCriteria<TInspector>
        where TInspector : IInspector
    {
        IAcceptanceCriteria<TInspector> SameAs<T>()
            where T : IConventionAcceptance<TInspector>, new();
        IAcceptanceCriteria<TInspector> OppositeOf<T>()
            where T : IConventionAcceptance<TInspector>, new();

        IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, bool>> evaluation);
        IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, object>> propertyExpression, IAcceptanceCriterion value);

        // special case for string, because it's actually an IEnumerable<char>, which makes it fall through
        // to the collection expectations
        IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, string>> propertyExpression, IAcceptanceCriterion value);

        // collections
        IAcceptanceCriteria<TInspector> Expect<TCollectionItem>(Expression<Func<TInspector, IEnumerable<TCollectionItem>>> property, ICollectionAcceptanceCriterion<TCollectionItem> value)
            where TCollectionItem : IInspector;

        IAcceptanceCriteria<TInspector> Any(params Action<IAcceptanceCriteria<TInspector>>[] criteriaAlterations);
        IAcceptanceCriteria<TInspector> Any(params IAcceptanceCriteria<TInspector>[] subCriteria);
        IAcceptanceCriteria<TInspector> Either(Action<IAcceptanceCriteria<TInspector>> criteriaAlterationA, Action<IAcceptanceCriteria<TInspector>> criteriaAlterationB);
        IAcceptanceCriteria<TInspector> Either(IAcceptanceCriteria<TInspector> subCriteriaA, IAcceptanceCriteria<TInspector> subCriteriaB);

        IEnumerable<IExpectation> Expectations { get; }
        bool Matches(IInspector inspector);
        
    }
}