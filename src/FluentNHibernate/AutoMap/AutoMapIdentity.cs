using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapIdentity : IAutoMapper
    {
        private Func<PropertyInfo, bool> findPropertyconvention = p => p.Name == "Id";
        private Func<PropertyInfo, string> columnConvention;

        public void SetConvention(Func<PropertyInfo, bool> findPropertyconvention, Func<PropertyInfo, string> columnConvention)
        {
            if (findPropertyconvention != null)
                this.findPropertyconvention = findPropertyconvention;

            this.columnConvention = columnConvention;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            return findPropertyconvention.Invoke(property);
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (columnConvention == null)
            {
                classMap.Id(ExpressionBuilder.Create<T>(property));
            }
            else
            {
                classMap.Id(ExpressionBuilder.Create<T>(property), columnConvention.Invoke(property));
            }
        }
    }
}