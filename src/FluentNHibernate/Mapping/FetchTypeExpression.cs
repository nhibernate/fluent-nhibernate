using System;

namespace FluentNHibernate.Mapping;

/// <summary>
/// Fetching strategies for associations that map to NHibernate's <c>fetch</c>
/// attribute with the <c>fetchMode</c> type (join, many-to-one, one-to-one), which
/// only supports <c>join</c> and <c>select</c>. Collections additionally support
/// <c>subselect</c> via <see cref="CollectionFetchTypeExpression{TParent}"/>.
/// </summary>
public class FetchTypeExpression<TParent>(TParent parent, Action<string> setter)
{
    private const string SubselectNotSupportedMessage =
        "Subselect fetching is not supported for join, many-to-one and one-to-one associations; " +
        "NHibernate only allows join or select fetching for these. Use Select or Join instead.";

    private protected readonly TParent Parent = parent;
    private protected readonly Action<string> Setter = setter;

    /// <summary>
    /// Join fetching
    /// </summary>
    public TParent Join()
    {
        Setter("join");
        return Parent;
    }

    /// <summary>
    /// Select fetching
    /// </summary>
    public TParent Select()
    {
        Setter("select");
        return Parent;
    }

    /// <summary>
    /// Subselect/subquery fetching. Only supported for collections; throws for
    /// join, many-to-one and one-to-one associations.
    /// </summary>
    public virtual TParent Subselect() => throw new NotSupportedException(SubselectNotSupportedMessage);
}
