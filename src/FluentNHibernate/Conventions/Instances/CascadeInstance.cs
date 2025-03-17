using System;

namespace FluentNHibernate.Conventions.Instances;

public class CascadeInstance(Action<string> setter) : ICascadeInstance
{
    public void All()
    {
        setter("all");
    }

    public void None()
    {
        setter("none");
    }

    public void SaveUpdate()
    {
        setter("save-update");
    }

    public void Delete()
    {
        setter("delete");
    }

    public void Merge()
    {
        setter("merge");
    }
}
