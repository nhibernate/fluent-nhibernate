using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapManyToOne : IAutoMapper
    {
        private Func<PropertyInfo, bool> findPropertyconvention = p => ((p.PropertyType.Namespace != "System") && (p.PropertyType.Namespace != "System.Collections.Generic"));

        public bool MapsProperty(PropertyInfo property)
        {
            if (property.CanWrite)
                return findPropertyconvention.Invoke(property);

            return false;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (!classMap.CanMapProperty(property))
                return;

            classMap.References(ExpressionBuilder.Create<T>(property));
        }
    }
}