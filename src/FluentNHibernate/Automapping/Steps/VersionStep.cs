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
                Name = member.Name,
                ContainingEntityType = classMap.Type,
            };

            version.SetDefaultValue("Type", GetDefaultType(member));
            version.AddDefaultColumn(new ColumnMapping { Name = member.Name });

            SetDefaultAccess(member, version);

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

        void SetDefaultAccess(Member member, VersionMapping mapping)
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess != Access.Property && resolvedAccess != Access.Unset)
            {
                // if it's a property or unset then we'll just let NH deal with it, otherwise
                // set the access to be whatever we determined it might be
                mapping.SetDefaultValue("Access", resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite)
                mapping.SetDefaultValue("Access", cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
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