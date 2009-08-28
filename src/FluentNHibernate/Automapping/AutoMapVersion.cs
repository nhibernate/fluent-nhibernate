using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class AutoMapVersion : IAutoMapper
    {
        private static readonly IList<string> ValidNames = new List<string> { "version", "timestamp" };
        private static readonly IList<Type> ValidTypes = new List<Type> { typeof(int), typeof(long), typeof(TimeSpan), typeof(byte[]) };

        public bool MapsProperty(PropertyInfo property)
        {
            return ValidNames.Contains(property.Name.ToLowerInvariant()) && ValidTypes.Contains(property.PropertyType);
        }

        public void Map(ClassMappingBase classMap, PropertyInfo property)
        {
            if (property.DeclaringType != classMap.Type || !(classMap is ClassMapping))
                return;

            var version = new VersionMapping
            {
                Name = property.Name,
            };

            version.SetDefaultValue("Type", GetDefaultType(property));
            version.AddDefaultColumn(new ColumnMapping { Name = property.Name });

            if (IsSqlTimestamp(property))
            {
                version.Columns.Each(x =>
                {
                    x.SqlType = "timestamp";
                    x.NotNull = true;
                });
                version.UnsavedValue = null;
            }

            ((ClassMapping)classMap).Version = version;
        }

        private bool IsSqlTimestamp(PropertyInfo property)
        {
            return property.PropertyType == typeof(byte[]);
        }

        private TypeReference GetDefaultType(PropertyInfo property)
        {
            if (IsSqlTimestamp(property))
                return new TypeReference("BinaryBlob");

            return new TypeReference(property.PropertyType);
        }
    }
}