using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapVersion : IAutoMapper
    {
        private static readonly IList<Type> versionTypes = new List<Type> { typeof(int), typeof(long), typeof(TimeSpan) };
        private readonly Func<PropertyInfo, bool> findPropertyconvention = p => (p.Name.ToLower() == "version" || p.Name.ToLower() == "timestamp") && versionTypes.Contains(p.PropertyType);

        public bool MapsProperty(PropertyInfo property)
        {
            return findPropertyconvention.Invoke(property);
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
        }

        public void Map(ClassMapping classMap, PropertyInfo property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            classMap.Version = new VersionMapping();
            classMap.Version.Name = property.Name;
            classMap.Version.Column = property.Name;
        }
    }
}