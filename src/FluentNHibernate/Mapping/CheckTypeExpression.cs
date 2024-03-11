using System;

namespace FluentNHibernate.Mapping;

public class CheckTypeExpression<TParent>(TParent parent, Action<string> setter)
{
    internal Action<string> setValue = setter;

    private readonly TParent parent = parent;

    public void None()
    {
        setValue("none");
    }

    public void RowCount()
    {
        setValue("rowcount");
    }
}
