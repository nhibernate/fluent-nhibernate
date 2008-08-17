using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapManyToOne : IAutoMapper
    {
        private Func<PropertyInfo, bool> findPropertyconvention = p => ((p.PropertyType.Namespace != "System") && (p.PropertyType.Namespace != "System.Collections.Generic"));
        private Func<PropertyInfo, string> columnConvention;

        public AutoMapManyToOne(Conventions conventions)
        {
            
        }

        public bool MapsProperty(PropertyInfo property)
        {
            if (property.CanWrite)
                return findPropertyconvention.Invoke(property);

            return false;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            classMap.References(ExpressionBuilder.Create<T>(property));
        }
    }
}