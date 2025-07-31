using System;

namespace FluentNHibernate.Automapping;

public class AutoMapType(Type type)
{
    public Type Type { get; set;} = type;
    public bool IsMapped { get; set; }
}
