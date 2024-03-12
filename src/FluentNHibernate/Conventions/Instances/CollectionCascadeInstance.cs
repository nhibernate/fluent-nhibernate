using System;

namespace FluentNHibernate.Conventions.Instances;

public class CollectionCascadeInstance(Action<string> setter) : CascadeInstance(setter), ICollectionCascadeInstance
{
    private readonly Action<string> setter = setter;

    public void AllDeleteOrphan()
    {
        setter("all-delete-orphan");
    }

    public void DeleteOrphan()
    {
        setter("delete-orphan");
    }
}
