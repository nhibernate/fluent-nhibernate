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
        private readonly IDictionary<string, Expression<Func<TMapping, object>>> mappings = new Dictionary<string, Expression<Func<TMapping, object>>>();

        public void Map(Expression<Func<TInspector, object>> inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
        {
            Map(ReflectionHelper.GetProperty(inspectorProperty), mappingProperty);
        }

        private void Map(PropertyInfo inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
        {
            mappings[inspectorProperty.Name] =  mappingProperty;
        }

        public void AutoMap()
        {
            var inspectorProperties = GetProperties(typeof(TInspector));
            var mappingProperties = GetProperties(typeof(TMapping));

            foreach (var property in inspectorProperties)
            {
                var propertyName = property.Name;
                var mappingProperty = mappingProperties.FirstOrDefault(x => x.Name == propertyName);

                if (mappingProperty != null)
                {
                    var expression = ExpressionBuilder.Create<TMapping>(mappingProperty);

                    Map(property, expression);
                }
            }
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            var properties = new List<PropertyInfo>();

            if (type.IsInterface)
            {
                foreach (var @interface in type.GetInterfaces())
                    properties.AddRange(GetProperties(@interface));
            }

            foreach (var property in type.GetProperties())
                properties.Add(property);

            return properties;
        }

        public Expression<Func<TMapping, object>> Get(PropertyInfo property)
        {
            if (mappings.ContainsKey(property.Name))
                return mappings[property.Name];

            throw new UnmappedPropertyException(typeof(TInspector), property.Name);
        }
    }
}