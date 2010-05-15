using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class VersionStep : IAutomappingStep
    {
        private static readonly IList<string> ValidNames = new List<string> { "version", "timestamp" };
        private static readonly IList<Type> ValidTypes = new List<Type> { typeof(int), typeof(long), typeof(TimeSpan), typeof(byte[]) };
        readonly IAutomappingConfiguration cfg;

        public VersionStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member)
        {
            return ValidNames.Contains(member.Name.ToLowerInvariant()) && ValidTypes.Contains(member.PropertyType);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (!(classMap is ClassMapping)) return;

            var version = new VersionMapping
            {
                Name = member.Name,
                ContainingEntityType = classMap.Type,
            };

            version.SetDefaultValue("Type", GetDefaultType(member));
            version.AddDefaultColumn(new ColumnMapping { Name = member.Name });

            if (member.IsProperty && !member.CanWrite)
                version.Access = cfg.GetAccessStrategyForReadOnlyProperty(member).ToString();

            if (IsSqlTimestamp(member))
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

        private bool IsSqlTimestamp(Member property)
        {
            return property.PropertyType == typeof(byte[]);
        }

        private TypeReference GetDefaultType(Member property)
        {
            if (IsSqlTimestamp(property))
                return new TypeReference("BinaryBlob");

            return new TypeReference(property.PropertyType);
        }
    }
}