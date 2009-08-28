using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class PropertyInspector : ColumnBasedInspector, IPropertyInspector
    {
        private readonly InspectorModelMapper<IPropertyInspector, PropertyMapping> propertyMappings = new InspectorModelMapper<IPropertyInspector, PropertyMapping>();
        private readonly PropertyMapping mapping;

        public PropertyInspector(PropertyMapping mapping)
            : base(mapping.Columns)
        {
            this.mapping = mapping;

            propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
            propertyMappings.Map(x => x.Nullable, "NotNull");
        }

        public bool Insert
        {
            get { return mapping.Insert; }
        }

        public bool Update
        {
            get { return mapping.Update; }
        }

        public string Formula
        {
            get { return mapping.Formula; }
        }

        public TypeReference Type
        {
            get { return mapping.Type; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public bool OptimisticLock
        {
            get { return mapping.OptimisticLock; }
        }

        public Generated Generated
        {
            get { return Generated.FromString(mapping.Generated); }
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

        public bool LazyLoad
        {
            get { return mapping.Lazy; }
        }

        public Access Access
        {
            get
            {
                if (mapping.IsSpecified("Access"))
                    return Access.FromString(mapping.Access);

                return Access.Unset;
            }
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool ReadOnly
        {
            get { return mapping.Insert && mapping.Update; }
        }

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }
    }
}