using System;

namespace FluentNHibernate.Mapping;

public class CollectionCascadeExpression<TParent> : CascadeExpression<TParent>
{
    private readonly TParent parent;
    private readonly Action<string> setter;

    public CollectionCascadeExpression(TParent parent, Action<string> setter)
        : base(parent, setter)
    {
        this.parent = parent;
        this.setter = setter;
    }

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
