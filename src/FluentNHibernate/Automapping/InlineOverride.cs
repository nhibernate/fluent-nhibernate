using System;

namespace FluentNHibernate.Automapping;

public class InlineOverride(Type type, Action<object> action)
{
    public bool CanOverride(Type otherType)
    {
        return type == otherType || otherType.IsSubclassOf(type);
    }

    public void Apply(object mapping)
    {
        action(mapping);
    }
}
