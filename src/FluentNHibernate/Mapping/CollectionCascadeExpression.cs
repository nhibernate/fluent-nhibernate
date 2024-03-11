using System;

namespace FluentNHibernate.Mapping;

public class CollectionCascadeExpression<TParent>(TParent parent, Action<string> setter)
    : CascadeExpression<TParent>(parent, setter)
{
    private readonly TParent parent = parent;
    private readonly Action<string> setter = setter;

    /// <summary>
    /// Cascade all actions, deleting any orphaned records
    /// </summary>
    public new TParent AllDeleteOrphan()
    {
        setter("all-delete-orphan");
        return parent;
    }

    /// <summary>
    /// Cascade deletes, deleting any orphaned records
    /// </summary>
    public new TParent DeleteOrphan()
    {
        setter("delete-orphan");
        return parent;
    }
}
