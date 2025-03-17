using System;

namespace FluentNHibernate.Automapping;

public class AutoMappingExpressions
{
    /// <summary>
    /// Determines whether a member is to be automapped. 
    /// </summary>
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override ShouldMap(Member)")]
    public Func<Member, bool> FindMembers;

    /// <summary>
    /// Determines whether a member is the identity of an entity.
    /// </summary>
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsId")]
    public Func<Member, bool> FindIdentity;

    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override GetParentSideForManyToMany")]
    public Func<Type, Type, Type> GetParentSideForManyToMany;

    [Obsolete("Use IgnoreBase<T> or IgnoreBase(Type): AutoMap.AssemblyOf<Entity>().IgnoreBase(typeof(Parent<>))", true)]
    public Func<Type, bool> IsBaseType;

    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsConcreteBaseType")]
    public Func<Type, bool> IsConcreteBaseType;
        
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsComponent")]
    public Func<Type, bool> IsComponentType;

    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override GetComponentColumnPrefix")]
    public Func<Member, string> GetComponentColumnPrefix;

    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsDiscriminated")]
    public Func<Type, bool> IsDiscriminated;

    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override GetDiscriminatorColumn")]
    public Func<Type, string> DiscriminatorColumn;

#pragma warning disable 612,618
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override IsDiscriminated", true)]
    public Func<Type, SubclassStrategy> SubclassStrategy;
#pragma warning restore 612,618

    /// <summary>
    /// Determines whether an abstract class is a layer supertype or part of a mapped inheritance hierarchy.
    /// </summary>
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override AbstractClassIsLayerSupertype")]
    public Func<Type, bool> AbstractClassIsLayerSupertype;

    /// <summary>
    /// Specifies the value column used in a table of simple types. 
    /// </summary>
    [Obsolete("Use an instance of IAutomappingConfiguration for configuration, and override SimpleTypeCollectionValueColumn")]
    public Func<Member, string> SimpleTypeCollectionValueColumn;
}

#pragma warning disable 612,618
// disabled obsolete warnings for this class

class ExpressionBasedAutomappingConfiguration(AutoMappingExpressions expressions)
    : DefaultAutomappingConfiguration
{
    public override bool ShouldMap(Member member)
    {
        if (expressions.FindMembers is not null)
            return expressions.FindMembers(member);

        return base.ShouldMap(member);
    }

    public override bool IsId(Member member)
    {
        if (expressions.FindIdentity is not null)
            return expressions.FindIdentity(member);

        return base.IsId(member);
    }

    public override Type GetParentSideForManyToMany(Type left, Type right)
    {
        if (expressions.GetParentSideForManyToMany is not null)
            return expressions.GetParentSideForManyToMany(left, right);

        return base.GetParentSideForManyToMany(left, right);
    }

    public override bool IsConcreteBaseType(Type type)
    {
        if (expressions.IsConcreteBaseType is not null)
            return expressions.IsConcreteBaseType(type);

        return base.IsConcreteBaseType(type);
    }

    public override bool IsComponent(Type type)
    {
        if (expressions.IsComponentType is not null)
            return expressions.IsComponentType(type);

        return base.IsComponent(type);
    }

    public override string GetComponentColumnPrefix(Member member)
    {
        if (expressions.GetComponentColumnPrefix is not null)
            return expressions.GetComponentColumnPrefix(member);

        return base.GetComponentColumnPrefix(member);
    }

    public override bool IsDiscriminated(Type type)
    {
        if (expressions.IsDiscriminated is not null)
            return expressions.IsDiscriminated(type);

        return base.IsDiscriminated(type);
    }

    public override string GetDiscriminatorColumn(Type type)
    {
        if (expressions.DiscriminatorColumn is not null)
            return expressions.DiscriminatorColumn(type);

        return base.GetDiscriminatorColumn(type);
    }

    public override bool AbstractClassIsLayerSupertype(Type type)
    {
        if (expressions.AbstractClassIsLayerSupertype is not null)
            return expressions.AbstractClassIsLayerSupertype(type);

        return base.AbstractClassIsLayerSupertype(type);
    }

    public override string SimpleTypeCollectionValueColumn(Member member)
    {
        if (expressions.SimpleTypeCollectionValueColumn is not null)
            return expressions.SimpleTypeCollectionValueColumn(member);

        return base.SimpleTypeCollectionValueColumn(member);
    }
}
#pragma warning restore 612,618
