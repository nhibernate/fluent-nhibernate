using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapVersion : IAutoMapper
    {
        private static readonly IList<Type> VersionTypes = new List<Type> { typeof(int), typeof(long), typeof(TimeSpan) };
        private readonly Func<PropertyInfo, bool> findPropertyconvention = p => (p.Name.ToLower() == "version" || p.Name.ToLower() == "timestamp") && VersionTypes.Contains(p.PropertyType);
        private readonly Func<PropertyInfo, string> columnConvention;
        private readonly ConventionOverrides conventions;

        public AutoMapVersion(ConventionOverrides conventions)
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