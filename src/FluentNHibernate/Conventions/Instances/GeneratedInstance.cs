using System;

namespace FluentNHibernate.Conventions.Instances;

/// <inheritdoc cref="IGeneratedInstance"/>
public class GeneratedInstance(Action<string> setter) : IGeneratedInstance
{
    /// <inheritdoc />
    public void Never()
    {
        setter("never");
    }

    /// <inheritdoc />
    public void Insert()
    {
        setter("insert");
    }

    /// <inheritdoc />
    public void Always()
    {
        setter("always");
    }
}
