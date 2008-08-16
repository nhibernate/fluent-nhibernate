using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapVersion : IAutoMapper
    {
        private Func<PropertyInfo, bool> findPropertyconvention = p => (p.Name.ToLower() == "version") || (p.Name.ToLower() == "timestamp");
        private Func<PropertyInfo, string> columnConvention;

        public bool MapsProperty(PropertyInfo property)
        {
            return findPropertyconvention.Invoke(property);
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            var verionMap = classMap
                .Version(ExpressionBuilder.Create<T>(property));

            if (columnConvention != null)
                verionMap.TheColumnNameIs(columnConvention.Invoke(property));

        }

        public void SetConvention(Func<PropertyInfo, bool> findPropertyconvention, Func<PropertyInfo, string> columnConvention)
        {
            if (findPropertyconvention != null)
                this.findPropertyconvention = findPropertyconvention;

            this.columnConvention = columnConvention;
        }
    }
}