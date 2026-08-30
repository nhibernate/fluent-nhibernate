using System;

namespace FluentNHibernate.Conventions.Instances;

public class FetchInstance(Action<string> setter) : IFetchInstance
{
    internal const string SubselectNotSupportedMessage =
        "Subselect fetching is not supported for join, many-to-one and one-to-one associations; " +
        "NHibernate only allows join or select fetching for these. Use Select or Join instead.";

    private protected readonly Action<string> Setter = setter;

    public void Join()
    {
        Setter("join");
    }

    public void Select()
    {
        Setter("select");
    }

    /// <summary>
    /// Subselect fetching. Only supported for collections; throws for join,
    /// many-to-one and one-to-one associations.
    /// </summary>
    public virtual void Subselect()
    {
        throw new NotSupportedException(SubselectNotSupportedMessage);
    }
}
