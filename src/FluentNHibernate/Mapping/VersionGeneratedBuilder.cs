using System;

namespace FluentNHibernate.Mapping;

public class VersionGeneratedBuilder<TParent>(TParent parent, Action<string> setter)
{
    public TParent Always()
    {
        setter("always");
        return parent;
    }

    public TParent Never()
    {
        setter("never");
        return parent;
    }
}
