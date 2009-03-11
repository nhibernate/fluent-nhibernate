using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapManyToOne : IAutoMapper
    {
        private Func<PropertyInfo, bool> findPropertyconvention = p => (
            p.PropertyType.Namespace != "System" && // ignore clr types (won't be entities)
            p.PropertyType.Namespace != "System.Collections.Generic" &&
            p.PropertyType.Namespace != "Iesi.Collections.Generic" &&
	    !p.PropertyType.IsEnum);

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

            classMap.References<object>(ExpressionBuilder.Create<T>(property));
        }
    }
}
