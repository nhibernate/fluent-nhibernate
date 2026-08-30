using System;

namespace FluentNHibernate.Conventions.Instances;

public class CollectionFetchInstance(Action<string> setter)
    : FetchInstance(setter)
{
    public override void Subselect()
    {
        Setter("subselect");
    }
}
