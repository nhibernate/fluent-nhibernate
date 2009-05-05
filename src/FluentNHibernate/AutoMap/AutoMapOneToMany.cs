using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapOneToMany : IAutoMapper
    {
        private readonly Func<PropertyInfo, bool> findPropertyconvention = 
            p => (p.PropertyType.Namespace == "System.Collections.Generic" ||
                  p.PropertyType.Namespace == "Iesi.Collections.Generic");

        public bool MapsProperty(PropertyInfo property)
        {
            if (property.CanWrite)
                return findPropertyconvention(property);

            return false;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (!classMap.CanMapProperty(property))
                return;

            var classMapType = typeof(ClassMap<T>);
            var hasManyMethod = classMapType.GetMethod("HasMany", new[] { typeof(Expression<Func<T, object>>) });
            var listType = property.PropertyType.GetGenericArguments()[0];
            var genericHasManyMethod = hasManyMethod.MakeGenericMethod(listType);
            genericHasManyMethod.Invoke(classMap, new object[] { ExpressionBuilder.Create<T>(property) });
        }
    }
}
