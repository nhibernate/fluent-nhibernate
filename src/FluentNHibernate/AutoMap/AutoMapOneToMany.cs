using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapOneToMany : IAutoMapper
    {
        public bool MapsProperty(PropertyInfo property)
        {
            return property.PropertyType.Namespace == "System.Collections.Generic";
        }

        public void Map<T>(ClassMap<T> classMap, PropertyInfo property)
        {
            var classMapType = typeof(ClassMap<T>);
            var hasManyMethod = classMapType.GetMethod("HasMany");
            var listType = property.PropertyType.GetGenericArguments()[0];
            var genericHasManyMethod = hasManyMethod.MakeGenericMethod(listType);
            genericHasManyMethod.Invoke(classMap, new object[] { ExpressionBuilder.Create<T>(property) });
        }
    }
}