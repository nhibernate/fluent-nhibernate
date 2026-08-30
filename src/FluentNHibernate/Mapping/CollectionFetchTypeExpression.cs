using System;

namespace FluentNHibernate.Mapping;

/// <summary>
/// Fetching strategies for collections, which map to NHibernate's <c>fetch</c>
/// attribute with the <c>collectionFetchMode</c> type. In addition to <c>join</c>
/// and <c>select</c>, collections also support <c>subselect</c>.
/// </summary>
public class CollectionFetchTypeExpression<TParent>(TParent parent, Action<string> setter)
    : FetchTypeExpression<TParent>(parent, setter)
{
    /// <summary>
    /// Subselect/subquery fetching
    /// </summary>
    public override TParent Subselect()
    {
        Setter("subselect");
        return Parent;
    }
}
