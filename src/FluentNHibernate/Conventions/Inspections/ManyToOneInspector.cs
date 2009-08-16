using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ManyToOneInspector : IManyToOneInspector
    {
        private readonly InspectorModelMapper<IManyToOneInspector, ManyToOneMapping> propertyMappings = new InspectorModelMapper<IManyToOneInspector, ManyToOneMapping>();
        private readonly ManyToOneMapping mapping;

        public ManyToOneInspector(ManyToOneMapping mapping)
        {
            this.mapping = mapping;
            propertyMappings.AutoMap();
            propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
        }

        public Access Access
        {
            get { return Access.FromString(mapping.Access); }
        }

        public NotFound NotFound
        {
            get { return NotFound.FromString(mapping.NotFound); }
        }

        public string PropertyRef
        {
            get { return mapping.PropertyRef; }
        }

        public bool Update
        {
            get { return mapping.Update; }
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

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                return mapping.Columns.UserDefined
                    .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                    .Cast<IColumnInspector>();
            }
        }

        public Cascade Cascade
        {
            get { return Cascade.FromString(mapping.Cascade); }
        }

        public TypeReference Class
        {
            get { return mapping.Class; }
        }

        public Fetch Fetch
        {
            get { return Fetch.FromString(mapping.Fetch); }
        }

        public string ForeignKey
        {
            get { return mapping.ForeignKey; }
        }

        public bool Insert
        {
            get { return mapping.Insert; }
        }

        public bool LazyLoad
        {
            get { return mapping.Lazy; }
        }
    }
}