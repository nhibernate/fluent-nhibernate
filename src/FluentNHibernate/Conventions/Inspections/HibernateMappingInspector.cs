using System;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class HibernateMappingInspector(HibernateMapping mapping) : IHibernateMappingInspector
{
    private readonly InspectorModelMapper<IHibernateMappingInspector, HibernateMapping> propertyMappings = new InspectorModelMapper<IHibernateMappingInspector, HibernateMapping>();

    public Type EntityType => mapping.Classes.First().Type;

    public string StringIdentifierForModel => mapping.Classes.First().Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public string Catalog => mapping.Catalog;

    public Access DefaultAccess => Access.FromString(mapping.DefaultAccess);

    public Cascade DefaultCascade => Cascade.FromString(mapping.DefaultCascade);

    public bool DefaultLazy => mapping.DefaultLazy;

    public bool AutoImport => mapping.AutoImport;

    public string Schema => mapping.Schema;
}
