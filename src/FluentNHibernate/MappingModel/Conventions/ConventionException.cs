using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.MappingModel.Conventions;

[Serializable]
public class ConventionException : Exception
{
    public ConventionException(string message, object conventionTarget) : base(message)
    {
        this.ConventionTarget = conventionTarget;
    }

    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    protected ConventionException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }

    public object ConventionTarget { get; }
}
