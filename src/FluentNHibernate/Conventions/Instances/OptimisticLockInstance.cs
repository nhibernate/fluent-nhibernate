using System;

namespace FluentNHibernate.Conventions.Instances;

public class OptimisticLockInstance(Action<string> setter) : IOptimisticLockInstance
{
    public void None()
    {
        setter("none");
    }

    public void Version()
    {
        setter("version");
    }

    public void Dirty()
    {
        setter("dirty");
    }

    public void All()
    {
        setter("all");
    }
}
