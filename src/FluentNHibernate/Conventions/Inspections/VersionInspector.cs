using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class VersionInspector : ColumnBasedInspector, IVersionInspector
    {
        private readonly InspectorModelMapper<IVersionInspector, VersionMapping> propertyMappings = new InspectorModelMapper<IVersionInspector, VersionMapping>();
        private readonly VersionMapping mapping;

        public VersionInspector(VersionMapping mapping)
            : base(mapping.Columns)
        {
            this.mapping = mapping;
            propertyMappings.Map(x => x.Nullable, "NotNull");
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool IsSet(Member property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public Access Access
        {
            get { return Access.FromString(mapping.Access); }
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                return mapping.Columns
                    .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                    .Cast<IColumnInspector>()
                    .ToList();
            }
        }

        public Generated Generated
        {
            get { return Generated.FromString(mapping.Generated); }
        }

        public string UnsavedValue
        {
            get { return mapping.UnsavedValue; }
        }

        public TypeReference Type
        {
            get { return mapping.Type; }
        }
    }
}