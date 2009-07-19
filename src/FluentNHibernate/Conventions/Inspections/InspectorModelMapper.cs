using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class InspectorModelMapper<TInspector, TMapping>
    {
        private readonly IDictionary<string, Expression<Func<TMapping, object>>> mappings = new Dictionary<string, Expression<Func<TMapping, object>>>();

        public void Map(Expression<Func<TInspector, object>> inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
        {
            Map(ReflectionHelper.GetProperty(inspectorProperty), mappingProperty);
        }

        private void Map(PropertyInfo inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
        {
            mappings.Add(inspectorProperty.Name, mappingProperty);
        }

        public void AutoMap()
        {
            var inspectorProperties = typeof(TInspector).GetProperties();
            var mappingProperties = typeof(TMapping).GetProperties();

            foreach (var property in inspectorProperties)
            {
                var propertyName = property.Name;
                var mappingProperty = mappingProperties.FirstOrDefault(x => x.Name == propertyName);
                var expression = ExpressionBuilder.Create<TMapping>(mappingProperty);

                if (mappingProperty != null)
                    Map(property, expression);
            }
        }

        public Expression<Func<TMapping, object>> Get(PropertyInfo property)
        {
            if (mappings.ContainsKey(property.Name))
                return mappings[property.Name];

            throw new UnmappedPropertyException(typeof(TInspector), property.Name);
        }
    }
}