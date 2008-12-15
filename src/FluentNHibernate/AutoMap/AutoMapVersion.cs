using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapVersion : IAutoMapper
    {
        private readonly Conventions conventions;
        private Func<PropertyInfo, bool> findPropertyconvention = p => (p.Name.ToLower() == "version") || (p.Name.ToLower() == "timestamp");
        private Func<PropertyInfo, string> columnConvention;

        public AutoMapVersion(Conventions conventions)
        {
            this.conventions = conventions;
            columnConvention = conventions.GetVersionColumnName;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            return findPropertyconvention.Invoke(property);
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (classMap is AutoJoinedSubClassPart<T>)
                return;

            var verionMap = classMap
                .Version(ExpressionBuilder.Create<T>(property));

            if (columnConvention != null)
                verionMap.TheColumnNameIs(columnConvention.Invoke(property));

        }
    }
}