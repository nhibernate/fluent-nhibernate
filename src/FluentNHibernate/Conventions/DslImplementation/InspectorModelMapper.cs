using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class InspectorModelMapper<TInspector, TMapping>
    {
        private readonly IDictionary<string, Expression<Func<TMapping, object>>> mappings = new Dictionary<string, Expression<Func<TMapping, object>>>();

        public void Map(Expression<Func<TInspector, object>> inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
        {
            mappings.Add(ReflectionHelper.GetProperty(inspectorProperty).Name, mappingProperty);
        }

        public Expression<Func<TMapping, object>> Get(PropertyInfo property)
        {
            if (mappings.ContainsKey(property.Name))
                return mappings[property.Name];

            throw new UnmappedPropertyException(typeof(TInspector), property.Name);
        }
    }
}