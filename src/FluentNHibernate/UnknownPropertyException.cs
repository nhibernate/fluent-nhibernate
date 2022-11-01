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

    public string Property { get; private set; }

    public Type Type { get; private set; }

    protected UnknownPropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        this.Type = Type.GetType(info.GetString("TypeFullName"));
        this.Property = info.GetString("Property");
    }

    [SecurityCritical]
    public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("TypeFullName", Type.FullName);
        info.AddValue("Property", Property);
    }
}
