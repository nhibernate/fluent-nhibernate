using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentNHibernate.Utils;

namespace FluentNHibernate;

[Serializable]
public abstract class Member : IEquatable<Member>
{
    public abstract string Name { get; }
    public abstract Type PropertyType { get; }
    public abstract bool CanWrite { get; }
    public abstract MemberInfo MemberInfo { get; }
    public abstract Type DeclaringType { get; }
    public abstract bool HasIndexParameters { get; }
    public abstract bool IsMethod { get; }
    public abstract bool IsField { get; }
    public abstract bool IsProperty { get; }
    public abstract bool IsAutoProperty { get; }
    public abstract bool IsPrivate { get; }
    public abstract bool IsProtected { get; }
    public abstract bool IsPublic { get; }
    public abstract bool IsInternal { get; }

    public bool Equals(Member other)
    {
        return Equals(other.MemberInfo.MetadataToken, MemberInfo.MetadataToken) && Equals(other.MemberInfo.Module, MemberInfo.Module);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (!(obj is Member)) return false;
        return Equals((Member)obj);
    }

    public override int GetHashCode()
    {
        return MemberInfo.GetHashCode() ^ 3;
    }

    public static bool operator ==(Member left, Member right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Member left, Member right)
    {
        return !Equals(left, right);
    }

    public abstract void SetValue(object target, object value);
    public abstract object GetValue(object target);
    public abstract bool TryGetBackingField(out Member backingField);
}

[Serializable]
internal class MethodMember(MethodInfo member) : Member
{
    private readonly MethodInfo member = member;
    Member backingField;

    public override void SetValue(object target, object value)
    {
        throw new NotSupportedException("Cannot set the value of a method Member.");
    }

    public override object GetValue(object target)
    {
        return member.Invoke(target, null);
    }

    public override bool TryGetBackingField(out Member field)
    {
        if (backingField is not null)
        {
            field = backingField;
            return true;
        }

        var name = Name;

        if (name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
            name = name.Substring(3);

        var reflectedField = DeclaringType.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        reflectedField = reflectedField ?? DeclaringType.GetField("_" + name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        reflectedField = reflectedField ?? DeclaringType.GetField("m_" + name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (reflectedField is null)
        {
            field = null;
            return false;
        }

        field = backingField = new FieldMember(reflectedField);
        return true;
    }

    public override string Name => member.Name;

    public override Type PropertyType => member.ReturnType;

    public override bool CanWrite => false;

    public override MemberInfo MemberInfo => member;

    public override Type DeclaringType => member.DeclaringType;

    public override bool HasIndexParameters => false;

    public override bool IsMethod => true;

    public override bool IsField => false;

    public override bool IsProperty => false;

    public override bool IsAutoProperty => false;

    public override bool IsPrivate => member.IsPrivate;

    public override bool IsProtected => member.IsFamily || member.IsFamilyAndAssembly;

    public override bool IsPublic => member.IsPublic;

    public override bool IsInternal => member.IsAssembly || member.IsFamilyAndAssembly;

    public bool IsCompilerGenerated => member.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any();

    public override string ToString()
    {
        return "{Method: " + member.Name + "}";
    }
}

[Serializable]
internal class FieldMember(FieldInfo member) : Member
{
    private readonly FieldInfo member = member;

    public override void SetValue(object target, object value)
    {
        member.SetValue(target, value);
    }

    public override object GetValue(object target)
    {
        return member.GetValue(target);
    }

    public override bool TryGetBackingField(out Member backingField)
    {
        backingField = null;
        return false;
    }

    public override string Name => member.Name;

    public override Type PropertyType => member.FieldType;

    public override bool CanWrite => true;

    public override MemberInfo MemberInfo => member;

    public override Type DeclaringType => member.DeclaringType;

    public override bool HasIndexParameters => false;

    public override bool IsMethod => false;

    public override bool IsField => true;

    public override bool IsProperty => false;

    public override bool IsAutoProperty => false;

    public override bool IsPrivate => member.IsPrivate;

    public override bool IsProtected => member.IsFamily || member.IsFamilyAndAssembly;

    public override bool IsPublic => member.IsPublic;

    public override bool IsInternal => member.IsAssembly || member.IsFamilyAndAssembly;

    public override string ToString()
    {
        return "{Field: " + member.Name + "}";
    }
}

[Serializable]
internal class PropertyMember : Member
{
    readonly PropertyInfo member;
    Member backingField;

    public PropertyMember(PropertyInfo member)
    {
        this.member = member;
        Get = GetMember(member.GetGetMethod(true));
        Set = GetMember(member.GetSetMethod(true));
    }

    MethodMember GetMember(MethodInfo method)
    {
        return (MethodMember)method?.ToMember();
    }

    public override void SetValue(object target, object value)
    {
        member.SetValue(target, value, null);
    }

    public override object GetValue(object target)
    {
        return member.GetValue(target, null);
    }

    public override bool TryGetBackingField(out Member field)
    {
        if (backingField is not null)
        {
            field = backingField;
            return true;
        }

        var reflectedField = DeclaringType.GetField(Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        reflectedField = reflectedField ?? DeclaringType.GetField("_" + Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        reflectedField = reflectedField ?? DeclaringType.GetField("m_" + Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (reflectedField is null)
        {
            field = null;
            return false;
        }

        field = backingField = new FieldMember(reflectedField);
        return true;
    }

    public override string Name => member.Name;

    public override Type PropertyType => member.PropertyType;

    public override bool CanWrite
    {
        get
        {
            // override the default reflection value here. Private setters aren't
            // considered "settable" in the same sense that public ones are. We can
            // use this to control the access strategy later
            if (IsAutoProperty && (Set is null || Set.IsPrivate))
                return false;

            return member.CanWrite;
        }
    }
    public override MemberInfo MemberInfo => member;

    public override Type DeclaringType => member.DeclaringType;

    public override bool HasIndexParameters => member.GetIndexParameters().Length > 0;

    public override bool IsMethod => false;

    public override bool IsField => false;

    public override bool IsProperty => true;

    public override bool IsAutoProperty =>
        (Get is not null && Get.IsCompilerGenerated) 
        || (Set is not null && Set.IsCompilerGenerated);

    public override bool IsPrivate => Get.IsPrivate;

    public override bool IsProtected => Get.IsProtected;

    public override bool IsPublic => Get.IsPublic;

    public override bool IsInternal => Get.IsInternal;

    public MethodMember Get { get; }

    public MethodMember Set { get; }

    public override string ToString()
    {
        return "{Property: " + member.Name + "}";
    }
}

public static class MemberExtensions
{
    public static Member ToMember(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is null)
            throw new NullReferenceException("Cannot create member from null.");
            
        return new PropertyMember(propertyInfo);
    }

    public static Member ToMember(this MethodInfo methodInfo)
    {
        if (methodInfo is null)
            throw new NullReferenceException("Cannot create member from null.");

        return new MethodMember(methodInfo);
    }

    public static Member ToMember(this FieldInfo fieldInfo)
    {
        if (fieldInfo is null)
            throw new NullReferenceException("Cannot create member from null.");

        return new FieldMember(fieldInfo);
    }

    public static Member ToMember(this MemberInfo memberInfo)
    {
        if (memberInfo is null)
            throw new NullReferenceException("Cannot create member from null.");

        if (memberInfo is PropertyInfo)
            return ((PropertyInfo)memberInfo).ToMember();
        if (memberInfo is FieldInfo)
            return ((FieldInfo)memberInfo).ToMember();
        if (memberInfo is MethodInfo)
            return ((MethodInfo)memberInfo).ToMember();

        throw new InvalidOperationException("Cannot convert MemberInfo '" + memberInfo.Name + "' to Member.");
    }

    public static IEnumerable<Member> GetInstanceFields(this Type type)
    {
        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            if (!field.Name.StartsWith("<"))
                yield return field.ToMember();
    }

    public static IEnumerable<Member> GetInstanceMethods(this Type type)
    {
        foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            if (!method.Name.StartsWith("get_") && !method.Name.StartsWith("set_") && method.ReturnType != typeof(void) && method.GetParameters().Length == 0)
                yield return method.ToMember();
    }

    public static IEnumerable<Member> GetInstanceProperties(this Type type)
    {
        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            yield return property.ToMember();
    }

    public static IEnumerable<Member> GetInstanceMembers(this Type type)
    {
        var members = new HashSet<Member>(new MemberEqualityComparer());

        type.GetInstanceProperties().Each(x => members.Add(x));
        type.GetInstanceFields().Each(x => members.Add(x));
        type.GetInstanceMethods().Each(x => members.Add(x));

        if (type.BaseType is not null && type.BaseType != typeof(object))
            type.BaseType.GetInstanceMembers().Each(x => members.Add(x));

        return members;
    }
}

public class MemberEqualityComparer : IEqualityComparer<Member>
{
    public bool Equals(Member x, Member y)
    {
        return x.MemberInfo.MetadataToken.Equals(y.MemberInfo.MetadataToken) && x.MemberInfo.Module.Equals(y.MemberInfo.Module);
    }

    public int GetHashCode(Member obj)
    {
        return obj.MemberInfo.MetadataToken.GetHashCode() & obj.MemberInfo.Module.GetHashCode();
    }
}
