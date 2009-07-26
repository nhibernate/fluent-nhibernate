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

        IAcceptanceCriteria<TInspector> Any(params Action<IAcceptanceCriteria<TInspector>>[] criteriaBuilders);
        IAcceptanceCriteria<TInspector> Either(Action<IAcceptanceCriteria<TInspector>> criteriaBuilderA, Action<IAcceptanceCriteria<TInspector>> criteriaBuilderB);

        IEnumerable<IExpectation> Expectations { get; }
        bool Matches(IInspector inspector);
    }
}