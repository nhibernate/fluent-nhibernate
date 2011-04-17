using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class VersionStep : IAutomappingStep
    {
        readonly IAutomappingConfiguration cfg;

        public VersionStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member)
        {
            return cfg.IsVersion(member);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (!(classMap is ClassMapping)) return;

            var version = new VersionMapping
            {
                ContainingEntityType = classMap.Type,
            };
            version.Set(x => x.Name, Layer.Defaults, member.Name);
            version.Set(x => x.Type, Layer.Defaults, GetDefaultType(member));
            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, member.Name);
            version.AddColumn(Layer.Defaults, columnMapping);

            SetDefaultAccess(member, version);

            if (IsSqlTimestamp(member))
            {
                version.Columns.Each(column =>
                {
                    column.Set(x => x.SqlType, Layer.Defaults, "timestamp");
                    column.Set(x => x.NotNull, Layer.Defaults, true);
                });
                version.Set(x => x.UnsavedValue, Layer.Defaults, null);
            }

            ((ClassMapping)classMap).Set(x => x.Version, Layer.Defaults, version);
        }

        void SetDefaultAccess(Member member, VersionMapping mapping)
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess != Access.Property && resolvedAccess != Access.Unset)
            {
                // if it's a property or unset then we'll just let NH deal with it, otherwise
                // set the access to be whatever we determined it might be
                mapping.Set(x => x.Access, Layer.Defaults, resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite)
                mapping.Set(x => x.Access, Layer.Defaults, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
        }

        static bool IsSqlTimestamp(Member property)
        {
            return property.PropertyType == typeof(byte[]);
        }

        static TypeReference GetDefaultType(Member property)
        {
            if (IsSqlTimestamp(property))
                return new TypeReference("BinaryBlob");

            return new TypeReference(property.PropertyType);
        }
    }
}