using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapComponent : IAutoMapper
    {
        private readonly Conventions conventions;

        public AutoMapComponent(Conventions conventions)
        {
            this.conventions = conventions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            return conventions.IsComponentType(property.PropertyType);
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            var componentType = property.PropertyType;
            var componentPart = CreateComponentPart(property, componentType, classMap);

            MapComponentProperties(property, componentType, componentPart);
        }

        private void MapComponentProperties(PropertyInfo componentProperty, Type componentType, object componentPart)
        {
            var mapMethod = GetMapMethod(componentType, componentPart);
            var columnNamePrefix = conventions.GetComponentColumnPrefix(componentProperty);

            foreach (var property in componentType.GetProperties())
            {
                if (property.PropertyType.IsEnum || property.GetIndexParameters().Length != 0) continue;

                var propertyMap = (IProperty)mapMethod.Invoke(componentPart, new[] {ExpressionBuilder.Create(property, componentType)});

                propertyMap.TheColumnNameIs(columnNamePrefix + property.Name);
            }
        }

        private object CreateComponentPart<T>(PropertyInfo property, Type componentType, AutoMap<T> classMap)
        {
            return InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(classMap, map => map.Component<object>(null, null), new object[] {ExpressionBuilder.Create<T>(property), null}, componentType);
        }

        private MethodInfo GetMapMethod(Type componentType, object componentPart)
        {
            var funcType = typeof(Func<,>).MakeGenericType(componentType, typeof(object));
            var mapType = typeof(Expression<>).MakeGenericType(funcType);
            return componentPart.GetType().GetMethod("Map", new[] { mapType });
        }
    }
}