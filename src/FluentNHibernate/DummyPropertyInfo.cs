using System;
using System.Globalization;
using System.Reflection;

namespace FluentNHibernate;

[Serializable]
public sealed class DummyPropertyInfo(string name, Type type) : PropertyInfo
{
    public override Module Module => null;

    public override int MetadataToken => Name.GetHashCode();

    public override object[] GetCustomAttributes(bool inherit)
    {
        return Array.Empty<object>();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        return false;
    }

    public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
    {
        return obj;
    }

    public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
    {}

    public override MethodInfo[] GetAccessors(bool nonPublic)
    {
        return Array.Empty<MethodInfo>();
    }

    public override MethodInfo GetGetMethod(bool nonPublic)
    {
        return null;
    }

    public override MethodInfo GetSetMethod(bool nonPublic)
    {
        return null;
    }

    public override ParameterInfo[] GetIndexParameters()
    {
        return Array.Empty<ParameterInfo>();
    }

    public override string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public override Type DeclaringType { get; } = type ?? throw new ArgumentNullException(nameof(type));

    public override Type ReflectedType => null;

    public override Type PropertyType => DeclaringType;

    public override PropertyAttributes Attributes => PropertyAttributes.None;

    public override bool CanRead => false;

    public override bool CanWrite => false;

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return Array.Empty<object>();
    }
}
