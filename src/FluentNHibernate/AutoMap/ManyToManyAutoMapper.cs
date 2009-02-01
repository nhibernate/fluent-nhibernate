using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class ManyToManyAutoMapper : IAutoMapper
    {
        private readonly Conventions conventions;

        public ManyToManyAutoMapper(Conventions conventions)
        {
            this.conventions = conventions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            var type = property.PropertyType;
            if (type.Namespace != "Iesi.Collections.Generic" &&
                type.Namespace != "System.Collections.Generic")
                return false;

            var hasInverse = GetInverseProperty(property) != null;
            return hasInverse;
        }

        private PropertyInfo GetInverseProperty(PropertyInfo property)
        {
            Type type = property.PropertyType;
            var inverseSide = type.GetGenericTypeDefinition()
                .MakeGenericType(property.DeclaringType);

            var argument = type.GetGenericArguments()[0];
            return argument.GetProperties()
                .Where(x => x.PropertyType == inverseSide)
                .FirstOrDefault();
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (!classMap.CanMapProperty(property))
                return;

            PropertyInfo inverseProperty = GetInverseProperty(property);

            Type parentSide = conventions.GetParentSideForManyToMany(property.DeclaringType, inverseProperty.DeclaringType);

            var classMapType = typeof(ClassMap<T>);
            var hasManyMethod = classMapType.GetMethod("HasManyToMany");
            var listType = property.PropertyType.GetGenericArguments()[0];
            var genericHasManyMethod = hasManyMethod.MakeGenericMethod(listType);
            object manyToManyPart = genericHasManyMethod.Invoke(classMap, new object[] { ExpressionBuilder.Create<T>(property) });

            if (parentSide != property.DeclaringType)//inverse
            {
                string manyTableName = conventions.GetManyToManyTableName(property.DeclaringType, parentSide);
                Type type = manyToManyPart.GetType();
                type.GetMethod("Inverse").Invoke(manyToManyPart, new object[0]);
                type.GetMethod("WithTableName").Invoke(manyToManyPart, new object[] { manyTableName, });
            }
        }
    }
}
