using System;

namespace FluentNHibernate.Mapping;

public class CheckTypeExpression<TParent>(TParent parent, Action<string> setter)
{
    readonly TParent parent = parent;

    public void None()
    {
        setter("none");
    }

    public void RowCount()
    {
        setter("rowcount");
    }
}
