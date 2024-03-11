using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class TypeReference
{
    public static readonly TypeReference Empty = new TypeReference("nop");

    private readonly Type innerType;

    public TypeReference(string name)
    {
        innerType = Type.GetType(name, false, true);
        Name = name;
    }

    public TypeReference(Type type)
    {
        innerType = type;
        Name = type.Name;
    }

    public string Name { get; }

    public bool IsEnum
    {
        get
        {
            if (innerType is null)
                return false;

            if (innerType.IsGenericType)
            {
                if (innerType.GetGenericTypeDefinition() == typeof(EnumStringType<>))
                {
                    return true;
                }
#pragma warning disable CS0618
                if (innerType.GetGenericTypeDefinition() == typeof(GenericEnumMapper<>))
#pragma warning restore CS0618
                {
                    return true;
                }
            }


            return innerType.IsEnum;
        }
    }

    public bool IsGenericType
    {
        get
        {
            if (innerType is null)
                return false;

            return innerType.IsGenericType;
        }
    }

    public bool IsGenericTypeDefinition
    {
        get
        {
            if (innerType is null)
                return false;

            return innerType.IsGenericTypeDefinition;
        }
    }

    public Type GetGenericTypeDefinition()
    {
        return innerType?.GetGenericTypeDefinition();
    }

    public Type GenericTypeDefinition
    {
        get { return GetGenericTypeDefinition(); }
    }

    public bool IsNullable
    {
        get { return GenericTypeDefinition == typeof(Nullable<>); }
    }

    public Type[] GetGenericArguments()
    {
        if (innerType is null)
            return Array.Empty<Type>();

        return innerType.GetGenericArguments();
    }

    public IEnumerable<Type> GenericArguments
    {
        get { return GetGenericArguments(); }
    }

    public override string ToString()
    {
        return innerType is null ? Name : innerType.AssemblyQualifiedName;
    }

    public bool Equals(TypeReference other)
    {
        if(ReferenceEquals(other, null))
            return false;
        if (other.innerType is null && innerType is null)
            return other.Name.Equals(Name);                        
        if (other.innerType is not null)
            return other.innerType.Equals(innerType);

        return false;
    }

    public bool Equals(Type other)
    {
        if (ReferenceEquals(other, null))
            return false;
        return other.Equals(innerType);
    }

    public bool Equals(string other)
    {
        if (ReferenceEquals(other, null))
            return false;
        return other.Equals(Name);
    }

    public override bool Equals(object obj)
    {
        if(obj is null)
            return false;
        if (obj.GetType() == typeof(TypeReference))
            return Equals((TypeReference)obj);
        if (obj is Type)
            return Equals((Type)obj);
        if (obj is string)
            return Equals((string)obj);

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((innerType is not null ? innerType.GetHashCode() : 0) * 397) ^ (Name is not null ? Name.GetHashCode() : 0);
        }
    }

    public Type GetUnderlyingSystemType()
    {
        return innerType;
    }

    public static bool operator ==(TypeReference original, Type type)
    {
        if (type is null)
            return false;
        if (original == (Type)null || original.innerType is null)
            return false;

        return original.innerType == type;
    }

    public static bool operator !=(TypeReference original, Type type)
    {
        return !(original == type);
    }

    public static bool operator ==(Type original, TypeReference type)
    {
        return type == original;
    }

    public static bool operator !=(Type original, TypeReference type)
    {
        return !(original == type);
    }

    public static bool operator ==(TypeReference original, TypeReference other)
    {
        return original.Equals(other);
    }

    public static bool operator !=(TypeReference original, TypeReference other)
    {
        return !(original == other);
    }
}
