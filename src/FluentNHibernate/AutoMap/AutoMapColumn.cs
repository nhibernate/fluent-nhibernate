using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapColumn : IAutoMapper
    {
        private readonly Conventions conventions;

        public AutoMapColumn(Conventions conventions)
        {
            this.conventions = conventions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            if (HasExplicitTypeConvention(property))
                return true;

            if (property.CanWrite)
                return property.PropertyType.Namespace == "System";

            return false;
        }

        private bool HasExplicitTypeConvention(PropertyInfo property)
        {
            var convention = conventions.FindConvention(property.PropertyType);

            if (convention is DefaultConvention || convention == null)
                return false;

            return true;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (!classMap.CanMapProperty(property))
                return;

            classMap.Map(ExpressionBuilder.Create<T>(property));
        }
    }
}