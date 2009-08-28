using System;
using System.Reflection;
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

        public bool IsSet(PropertyInfo property)
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

        public IDefaultableEnumerable<IColumnInspector> Columns
        {
            get
            {
                var items = new DefaultableList<IColumnInspector>();

                foreach (var column in mapping.Columns.UserDefined)
                    items.Add(new ColumnInspector(mapping.ContainingEntityType, column));

                return items;
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