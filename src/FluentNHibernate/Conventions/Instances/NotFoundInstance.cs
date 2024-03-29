using System;

namespace FluentNHibernate.Conventions.Instances;

public class NotFoundInstance(Action<string> setter) : INotFoundInstance
{
    public void Ignore()
    {
        setter("ignore");
    }

    public void Exception()
    {
        setter("exception");
    }
}
