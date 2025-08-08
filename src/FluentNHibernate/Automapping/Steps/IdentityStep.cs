using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Automapping.Steps;

public class IdentityStep(IAutomappingConfiguration cfg) : IAutomappingStep
{
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
        new GeneratorBuilder(generatorMapping, property.PropertyType, Layer.Defaults).SetDefault();
        return generatorMapping;
    }
}
