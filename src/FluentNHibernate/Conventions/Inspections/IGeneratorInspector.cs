using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections;

public interface IGeneratorInspector : IInspector
{
    string Class { get; }
    IDictionary<string, string> Params { get; }
}

public class GeneratorInspector(GeneratorMapping mapping) : IGeneratorInspector
{
    readonly InspectorModelMapper<IGeneratorInspector, GeneratorMapping> propertyMappings = new InspectorModelMapper<IGeneratorInspector, GeneratorMapping>();

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Class;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public string Class => mapping.Class;

    public IDictionary<string, string> Params => new Dictionary<string, string>(mapping.Params);
}
