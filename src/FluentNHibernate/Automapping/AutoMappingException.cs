using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.Automapping;

[Serializable]
public class AutoMappingException : Exception
{
    public AutoMappingException(string message)
        : base(message)
    { }

    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    protected AutoMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
