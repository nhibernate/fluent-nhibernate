using System;

namespace FluentNHibernate.Mapping;

public class PolymorphismBuilder<T>(T parent, Action<string> setter)
{
    /// <summary>
    /// Implicit polymorphism
    /// </summary>
    public T Implicit()
    {
        setter("implicit");
        return parent;
    }

    /// <summary>
    /// Explicit polymorphism
    /// </summary>
    public T Explicit()
    {
        setter("explicit");
        return parent;
    }
}
