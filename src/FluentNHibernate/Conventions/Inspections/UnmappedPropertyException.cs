using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.Conventions.Inspections;

[Serializable]
public class UnmappedPropertyException : Exception
{
    public UnmappedPropertyException(Type type, string name)
        : base("Unmapped property '" + name + "' on type '" + type.Name + "'")
    {}

    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    protected UnmappedPropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {}
}
