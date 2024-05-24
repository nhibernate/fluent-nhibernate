using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Conventions.Inspections;

public class InspectorModelMapper<TInspector, TMapping>
{
    readonly Dictionary<string, string> mappings = new();

    public void Map(Expression<Func<TInspector, object>> inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
    {
        Map(inspectorProperty.ToMember(), mappingProperty);
    }

    public void Map(Expression<Func<TInspector, object>> inspectorProperty, string mappingProperty)
    {
        mappings[inspectorProperty.ToMember().Name] = mappingProperty;
    }

    void Map(Member inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
    {
        mappings[inspectorProperty.Name] =  mappingProperty.ToMember().Name;
    }

    public string Get(Member property)
    {
        if (mappings.TryGetValue(property.Name, out var mapping))
            return mapping;

        return property.Name;
    }
}
