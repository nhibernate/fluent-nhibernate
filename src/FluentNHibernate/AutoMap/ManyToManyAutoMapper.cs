using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class ManyToManyAutoMapper : IAutoMapper
    {
        public bool MapsProperty(PropertyInfo property)
        {
            var type = property.PropertyType;
            if (type.Namespace != "Iesi.Collections.Generic" &&
                type.Namespace != "System.Collections.Generic")
                return false;

            var inverseSide = type.GetGenericTypeDefinition()
                .MakeGenericType(property.DeclaringType);

            var argument = type.GetGenericArguments()[0];
            var hasInverse = argument.GetProperties()
                                 .Where(x => x.PropertyType == inverseSide)
                                 .Count() > 0;
            return hasInverse;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            var classMapType = typeof(ClassMap<T>);
            var hasManyMethod = classMapType.GetMethod("HasManyToMany");
            var listType = property.PropertyType.GetGenericArguments()[0];
            var genericHasManyMethod = hasManyMethod.MakeGenericMethod(listType);
            genericHasManyMethod.Invoke(classMap, new object[] { ExpressionBuilder.Create<T>(property) });
        }
    }
}
