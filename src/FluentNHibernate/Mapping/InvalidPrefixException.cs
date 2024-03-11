using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.Mapping;

/// <summary>
/// Thrown when a prefix is specified for an access-strategy that isn't supported.
/// </summary>
[Serializable]
public class InvalidPrefixException : Exception
{
    public InvalidPrefixException(string message) : base(message)
    { }

    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    protected InvalidPrefixException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
