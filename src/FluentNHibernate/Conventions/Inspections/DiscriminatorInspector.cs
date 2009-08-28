using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class DiscriminatorInspector : ColumnBasedInspector, IDiscriminatorInspector
    {
        private readonly InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping> propertyMappings = new InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping>();
        private readonly DiscriminatorMapping mapping;

        public DiscriminatorInspector(DiscriminatorMapping mapping)
            : base(mapping.Columns)
        {
            this.mapping = mapping;
            propertyMappings.Map(x => x.Nullable, "NotNull");
        }

        public bool Insert
        {
            get { return mapping.Insert; }
        }

        public bool Force
        {
            get { return mapping.Force; }
        }

        public string Formula
        {
            get { return mapping.Formula; }
        }

        public TypeReference Type
        {
            get { return mapping.Type; }
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Type.Name; }
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

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }
    }
}