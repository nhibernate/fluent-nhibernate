using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;

namespace FluentNHibernate.Conventions.Inspections
{
    public class InspectorModelMapper<TInspector, TMapping>
    {
        private readonly IDictionary<string, string> mappings = new Dictionary<string, string>();

        public void Map(Expression<Func<TInspector, object>> inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
        {
            Map(ReflectionHelper.GetProperty(inspectorProperty), mappingProperty);
        }

        public void Map(Expression<Func<TInspector, object>> inspectorProperty, string mappingProperty)
        {
            mappings[inspectorProperty.ToPropertyInfo().Name] = mappingProperty;
        }

        private void Map(PropertyInfo inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
        {
            mappings[inspectorProperty.Name] =  mappingProperty.ToPropertyInfo().Name;
        }

        public string Get(PropertyInfo property)
        {
            if (mappings.ContainsKey(property.Name))
                return mappings[property.Name];

            return property.Name;
        }
    }
}