using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using NHibernate.Type;

namespace FluentNHibernate.Mapping;

public abstract class FilterDefinition : IFilterDefinition
{
    private string filterCondition;
    private readonly IDictionary<string, IType> parameters;

    protected FilterDefinition()
    {
        parameters = new Dictionary<string, IType>();
    }

    public string Name { get; private set; }

    public IEnumerable<KeyValuePair<string, IType>> Parameters => parameters;

    public FilterDefinition WithName(string name)
    {
        Name = name;
        return this;
    }

    public FilterDefinition WithCondition(string condition)
    {
        filterCondition = condition;
        return this;
    }

    public FilterDefinition AddParameter(string name, IType type)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("The name is mandatory", nameof(name));
        if (type is null) throw new ArgumentNullException(nameof(type));
        parameters.Add(name, type);
        return this;
    }

    FilterDefinitionMapping IFilterDefinition.GetFilterMapping()
    {
        var mapping = new FilterDefinitionMapping();
        mapping.Set(x => x.Name, Layer.Defaults, Name);
        mapping.Set(x => x.Condition, Layer.Defaults, filterCondition);
        foreach (var pair in Parameters)
        {
            mapping.Parameters.Add(pair);
        }
        return mapping;
    }

    HibernateMapping IFilterDefinition.GetHibernateMapping()
    {
        return new HibernateMapping();
    }
}
