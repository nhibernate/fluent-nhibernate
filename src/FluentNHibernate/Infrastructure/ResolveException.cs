using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.Infrastructure;

[Serializable]
public class ResolveException : Exception
{
    public ResolveException(Type type)
        : base("Unable to resolve dependency: '" + type.FullName + "'")
    {}

    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    protected ResolveException(SerializationInfo info, StreamingContext context) : base(info, context)
    {}
}
