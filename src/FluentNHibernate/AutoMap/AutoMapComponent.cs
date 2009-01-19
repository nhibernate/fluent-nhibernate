using System;
using System.Linq.Expressions;
using System.Reflection;

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

            MapComponentProperties(componentType, componentPart);
        }

        private void MapComponentProperties(Type componentType, object componentPart)
        {
            var mapMethod = GetMapMethod(componentType, componentPart);

            foreach (var p in componentType.GetProperties())
            {
                if (p.PropertyType.IsEnum || p.GetIndexParameters().Length != 0) continue;

                mapMethod.Invoke(componentPart, new[] {ExpressionBuilder.Create(p, componentType)});
            }
        }

        private object CreateComponentPart<T>(PropertyInfo property, Type componentType, AutoMap<T> classMap)
        {
            var componentMethod = typeof(AutoMap<T>).GetMethod("Component").MakeGenericMethod(componentType);

            return componentMethod.Invoke(classMap, new object[] { ExpressionBuilder.Create<T>(property), null });
        }

        private MethodInfo GetMapMethod(Type componentType, object componentPart)
        {
            var funcType = typeof(Func<,>).MakeGenericType(componentType, typeof(object));
            var mapType = typeof(Expression<>).MakeGenericType(funcType);
            return componentPart.GetType().GetMethod("Map", new[] { mapType });
        }
    }
}