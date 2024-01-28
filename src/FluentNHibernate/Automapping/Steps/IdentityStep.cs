using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Automapping.Steps;

public class IdentityStep : IAutomappingStep
{
    private readonly IAutomappingConfiguration cfg;
    readonly List<Type> identityCompatibleTypes = new List<Type> { typeof(long), typeof(int), typeof(short), typeof(byte) };

    public IdentityStep(IAutomappingConfiguration cfg)
    {
        this.cfg = cfg;
    }

    public bool ShouldMap(Member member)
    {
        return cfg.IsId(member);
    }

    public void Map(ClassMappingBase classMap, Member member)
    {
        if (!(classMap is ClassMapping)) return;

        var idMapping = new IdMapping { ContainingEntityType = classMap.Type };
        var columnMapping = new ColumnMapping();
        columnMapping.Set(x => x.Name, Layer.Defaults, member.Name);
        idMapping.AddColumn(Layer.Defaults, columnMapping);
        idMapping.Set(x => x.Name, Layer.Defaults, member.Name);
        idMapping.Set(x => x.Type, Layer.Defaults, new TypeReference(member.PropertyType));
        idMapping.Member = member;
        idMapping.Set(x => x.Generator, Layer.Defaults, GetDefaultGenerator(member));

        columnMapping.TryApplyAttributesFrom(member.MemberInfo,isIdColumn:true);

        SetDefaultAccess(member, idMapping);

        ((ClassMapping)classMap).Set(x => x.Id, Layer.Defaults, idMapping);        
    }

    void SetDefaultAccess(Member member, IdMapping mapping)
    {
        var resolvedAccess = MemberAccessResolver.Resolve(member);

        if (resolvedAccess != Access.Property && resolvedAccess != Access.Unset)
        {
            // if it's a property or unset then we'll just let NH deal with it, otherwise
            // set the access to be whatever we determined it might be
            mapping.Set(x => x.Access, Layer.Defaults, resolvedAccess.ToString());
        }

        if (member.IsProperty && !member.CanWrite)
            mapping.Set(x => x.Access, Layer.Defaults, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
    }
        
    GeneratorMapping GetDefaultGenerator(Member property)
    {
        var generatorMapping = new GeneratorMapping();
        var defaultGenerator = new GeneratorBuilder(generatorMapping, property.PropertyType, Layer.Defaults);

        if (property.PropertyType == typeof(Guid))
            defaultGenerator.GuidComb();
        else if (identityCompatibleTypes.Contains(property.PropertyType))
            defaultGenerator.Identity();
        else
            defaultGenerator.Assigned();

        generatorMapping.TryApply(property.MemberInfo.GetCustomAttribute<DatabaseGeneratedAttribute>());
        
        return generatorMapping;
    }
}
