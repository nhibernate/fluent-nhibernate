using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapOneToMany : IAutoMapper
    {
        private Func<PropertyInfo, bool> findPropertyconvention = 
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
            var classMapType = typeof(ClassMap<T>);
            var hasManyMethod = classMapType.GetMethod("HasMany");
            var listType = property.PropertyType.GetGenericArguments()[0];
            var genericHasManyMethod = hasManyMethod.MakeGenericMethod(listType);
            genericHasManyMethod.Invoke(classMap, new object[] { ExpressionBuilder.Create<T>(property) });
        }
    }
}
