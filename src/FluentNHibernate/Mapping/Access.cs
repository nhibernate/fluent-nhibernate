﻿using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Mapping;

public class Access : IEquatable<Access>
{
    public static readonly Access Unset = new Access("");
    public static readonly Access Field = new Access("field");
    public static readonly Access BackField = new Access("backfield");
    public static readonly Access Property = new Access("property");
    public static readonly Access ReadOnlyProperty = new Access("nosetter");
    public static readonly Access NoOp = new Access("noop");
    public static readonly Access None = new Access("none");

    public static Access CamelCaseField()
    {
        return CamelCaseField(CamelCasePrefix.None);
    }

    public static Access CamelCaseField(CamelCasePrefix prefix)
    {
        return new Access("field", NamingStrategy.FromString("camelcase" + prefix));
    }

    public static Access LowerCaseField()
    {
        return LowerCaseField(LowerCasePrefix.None);
    }

    public static Access LowerCaseField(LowerCasePrefix prefix)
    {
        return new Access("field", NamingStrategy.FromString("lowercase" + prefix));
    }

    public static Access PascalCaseField(PascalCasePrefix prefix)
    {
        return new Access("field", NamingStrategy.FromString("pascalcase" + prefix));
    }

    public static Access ReadOnlyPropertyThroughCamelCaseField()
    {
        return ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix.None);
    }

    public static Access ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix prefix)
    {
        return new Access("nosetter", NamingStrategy.FromString("camelcase" + prefix));
    }

    public static Access ReadOnlyPropertyThroughLowerCaseField()
    {
        return ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix.None);
    }

    public static Access ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix prefix)
    {
        return new Access("nosetter", NamingStrategy.FromString("lowercase" + prefix));
    }

    public static Access ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix prefix)
    {
        return new Access("nosetter", NamingStrategy.FromString("pascalcase" + prefix));
    }

    public static Access ReadOnlyPropertyWithField(NamingStrategy namingStrategy)
    {
        return new Access("nosetter." + namingStrategy);
    }

    public static Access Using(string value)
    {
        return new Access(value);
    }

    public static Access Using(Type accessorType)
    {
        return Using(accessorType.AssemblyQualifiedName);
    }

    public static Access Using<T>()
    {
        return Using(typeof(T));
    }

    readonly string value;

    Access(string value)
    {
        this.value = value;
    }

    Access(string value, NamingStrategy strategy)
    {
        this.value = value + "." + strategy;
    }

    public override bool Equals(object obj)
    {
        if (obj is Access) return Equals((Access)obj);

        return base.Equals(obj);
    }

    public bool Equals(Access other)
    {
        return Equals(other.value, value);
    }

    public override int GetHashCode()
    {
        return (value is not null ? value.GetHashCode() : 0);
    }

    public static bool operator ==(Access x, Access y)
    {
        return x.Equals(y);
    }

    public static bool operator !=(Access x, Access y)
    {
        return !(x == y);
    }

    public override string ToString()
    {
        return value;
    }

    public static Access FromString(string value)
    {
        return new Access(value);
    }
}
