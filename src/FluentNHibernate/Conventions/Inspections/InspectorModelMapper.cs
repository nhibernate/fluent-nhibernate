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
            Map(ReflectionHelper.GetProperty(inspectorProperty).ToMember(), mappingProperty);
        }

        public void Map(Expression<Func<TInspector, object>> inspectorProperty, string mappingProperty)
        {
            mappings[inspectorProperty.ToMember().Name] = mappingProperty;
        }

        private void Map(Member inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
        {
            mappings[inspectorProperty.Name] =  mappingProperty.ToMember().Name;
        }

        public string Get(Member property)
        {
            if (mappings.ContainsKey(property.Name))
                return mappings[property.Name];

            return property.Name;
        }
    }
}