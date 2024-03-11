using System;
using System.Runtime.Serialization;
using System.Security;

namespace FluentNHibernate;

[Serializable]
public class UnknownPropertyException : Exception
{
    public UnknownPropertyException(Type classType, string propertyName)
        : base("Could not find property '" + propertyName + "' on '" + classType.FullName + "'")
    {
        Type = classType;
        Property = propertyName;
    }

    public string Property { get; }

    public Type Type { get; }

    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    protected UnknownPropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        this.Type = Type.GetType(info.GetString("TypeFullName"));
        this.Property = info.GetString("Property");
    }

#pragma warning disable 809
    [SecurityCritical]
    [Obsolete("This API supports obsolete formatter-based serialization and will be removed in a future version")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("TypeFullName", Type.FullName);
        info.AddValue("Property", Property);
    }
#pragma warning restore 809
}
