using System;
using System.Globalization;
using System.Reflection;

namespace FluentNHibernate;

internal sealed class DummyMethodInfo : MethodInfo
{
    public DummyMethodInfo(string name, Type type)
    {
        this.Name = name;
        this.ReturnType = type;
    }

    public override Type ReturnType { get; }

    public override object[] GetCustomAttributes(bool inherit)
    {
        return Array.Empty<object>();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        return false;
    }

    public override ParameterInfo[] GetParameters()
    {
        return Array.Empty<ParameterInfo>();
    }

    public override MethodImplAttributes GetMethodImplementationFlags()
    {
        return MethodImplAttributes.Managed;
    }

    public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
    {
        return obj;
    }

    public override MethodInfo GetBaseDefinition()
    {
        return null;
    }

    public override ICustomAttributeProvider ReturnTypeCustomAttributes => null;

    public override string Name { get; }

    public override Type DeclaringType => null;

    public override Type ReflectedType => null;

    public override RuntimeMethodHandle MethodHandle => new();

    public override MethodAttributes Attributes => MethodAttributes.Public;

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return Array.Empty<object>();
    }
}
