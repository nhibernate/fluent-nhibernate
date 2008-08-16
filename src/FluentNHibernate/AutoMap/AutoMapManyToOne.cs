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

        public bool MapsProperty(PropertyInfo property)
        {
            if (property.CanWrite)
                return findPropertyconvention.Invoke(property);

            return false;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (columnConvention == null)
            {
                classMap.References(ExpressionBuilder.Create<T>(property));
            }
            else
            {
                classMap.References(ExpressionBuilder.Create<T>(property), columnConvention.Invoke(property));
            }
        }

        public void SetConvention(Func<PropertyInfo, bool> findPropertyconvention, Func<PropertyInfo, string> columnConvention)
        {
            if (findPropertyconvention != null)
                this.findPropertyconvention = findPropertyconvention;

            this.columnConvention = columnConvention;
        }
    }
}