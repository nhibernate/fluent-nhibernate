using System;
using System.Globalization;
using System.Reflection;

namespace FluentNHibernate;

[Serializable]
public sealed class DummyPropertyInfo : PropertyInfo
{
    public DummyPropertyInfo(string name, Type type)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        DeclaringType = type ?? throw new ArgumentNullException(nameof(type));
    }

    public override Module Module
    {
        get { return null; }
    }

    public override int MetadataToken
    {
        get { return Name.GetHashCode(); }
    }

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

    public override string Name { get; }

    public override Type DeclaringType { get; }

    public override Type ReflectedType
    {
        get { return null; }
    }

    public override Type PropertyType
    {
        get { return DeclaringType; }
    }

    public override PropertyAttributes Attributes
    {
        get { return PropertyAttributes.None; }
    }

    public override bool CanRead
    {
        get { return false; }
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return Array.Empty<object>();
    }
}
