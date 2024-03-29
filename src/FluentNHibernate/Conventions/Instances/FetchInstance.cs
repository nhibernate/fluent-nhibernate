using System;

namespace FluentNHibernate.Conventions.Instances;

public class FetchInstance(Action<string> setter) : IFetchInstance
{
    public void Join()
    {
        setter("join");
    }

    public void Select()
    {
        setter("select");
    }

    public void Subselect()
    {
        setter("subselect");
    }
}
