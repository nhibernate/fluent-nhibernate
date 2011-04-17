using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Mapping;
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

        public bool LazyLoad
        {
            get { return mapping.Lazy; }
        }

        public Access Access
        {
            get
            {
                if (mapping.Access != null)
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

        public Member Property
        {
            get { return mapping.Member; }
        }

        public bool IsSet(Member property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }
    }
}