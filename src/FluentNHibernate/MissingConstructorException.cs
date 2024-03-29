using System;
using System.Runtime.Serialization;

namespace FluentNHibernate;

[Serializable]
public class MissingConstructorException : Exception
{
    public MissingConstructorException(Type type)
        : base("'" + type.AssemblyQualifiedName + "' is missing a parameterless constructor.")
    { }

    public MissingConstructorException(Type type, Exception innerException)
        : base("'" + type.AssemblyQualifiedName + "' is missing a parameterless constructor.", innerException)
    { }

    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    protected MissingConstructorException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
