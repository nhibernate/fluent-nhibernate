using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class AutoMapVersion : IAutoMapper
    {
        private static readonly IList<Type> versionTypes = new List<Type> { typeof(int), typeof(long), typeof(TimeSpan) };
        private readonly Func<PropertyInfo, bool> findPropertyconvention = p => (p.Name.ToLower() == "version" || p.Name.ToLower() == "timestamp") && versionTypes.Contains(p.PropertyType);

        public bool MapsProperty(PropertyInfo property)
        {
            return findPropertyconvention.Invoke(property);
        }

        public void Map(ClassMappingBase classMap, PropertyInfo property)
        {
            if (property.DeclaringType != classMap.Type || !(classMap is ClassMapping))
                return;

            ((ClassMapping)classMap).Version = new VersionMapping
            {
                Name = property.Name,
                Column = property.Name
            };
        }
    }
}