using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapColumn : IAutoMapper
    {
        public AutoMapColumn(Conventions conventions)
        {
        }

        public bool MapsProperty(PropertyInfo property)
        {
            if (property.CanWrite)
                return property.PropertyType.Namespace == "System";

            return false;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            classMap.Map(ExpressionBuilder.Create<T>(property));
        }
    }
}