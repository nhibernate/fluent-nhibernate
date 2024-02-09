using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping;

public class DefaultAutomappingConfiguration : IAutomappingConfiguration
{
    public virtual bool ShouldMap(Member member)
    {
        return member.IsProperty && member.IsPublic;
    }

    public virtual bool ShouldMap(Type type)
    {
        return !type.ClosesInterface(typeof(IAutoMappingOverride<>)) &&
               !type.HasInterface(typeof(IMappingProvider)) &&
               !type.IsNestedPrivate && 
               !type.IsDefined(typeof(CompilerGeneratedAttribute), false)
               && type.IsClass;
    }

    public virtual bool IsId(Member member)
    {
        if (member.MemberInfo.GetCustomAttribute<KeyAttribute>() != null)
        {
            return true;
        }
        return member.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
    }

    public virtual Access GetAccessStrategyForReadOnlyProperty(Member member)
    {
        return MemberAccessResolver.Resolve(member);
    }

    public virtual Type GetParentSideForManyToMany(Type left, Type right)
    {
        return left.FullName.CompareTo(right.FullName) < 0 ? left : right;
    }

    public virtual bool IsConcreteBaseType(Type type)
    {
        return false;
    }

    public virtual bool IsComponent(Type type)
    {
        return false;
    }

    public virtual string GetComponentColumnPrefix(Member member)
    {
        return member.Name;
    }

    public virtual bool IsDiscriminated(Type type)
    {
        return false;
    }

    public virtual string GetDiscriminatorColumn(Type type)
    {
        return "discriminator";
    }

    [Obsolete("Use IsDiscriminated instead.", true)]
    public SubclassStrategy GetSubclassStrategy(Type type)
    {
        throw new NotSupportedException();
    }

    public virtual bool AbstractClassIsLayerSupertype(Type type)
    {
        return true;
    }

    public virtual string SimpleTypeCollectionValueColumn(Member member)
    {
        return "Value";
    }

    public virtual bool IsVersion(Member member)
    {
        var validNames = new List<string> { "version", "timestamp" };
        var validTypes = new List<Type> { typeof(int), typeof(long), typeof(TimeSpan), typeof(byte[]) };

        return validNames.Contains(member.Name.ToLowerInvariant()) && validTypes.Contains(member.PropertyType);
    }

    public virtual IEnumerable<IAutomappingStep> GetMappingSteps(AutoMapper mapper, IConventionFinder conventionFinder)
    {
        return new IAutomappingStep[]
        {
            new IdentityStep(this),
            new VersionStep(this),
            new ComponentStep(this),
            new PropertyStep(conventionFinder, this),
            new HasManyToManyStep(this),
            new ReferenceStep(this),
            new HasManyStep(this)
        };
    }
}
