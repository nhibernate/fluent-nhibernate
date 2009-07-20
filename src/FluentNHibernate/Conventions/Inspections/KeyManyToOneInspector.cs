using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections
{
    public class KeyManyToOneInspector : IKeyManyToOneInspector
    {
        private readonly InspectorModelMapper<IKeyManyToOneInspector, KeyManyToOneMapping> mappedProperties = new InspectorModelMapper<IKeyManyToOneInspector, KeyManyToOneMapping>();
        private readonly KeyManyToOneMapping mapping;

        public KeyManyToOneInspector(KeyManyToOneMapping mapping)
        {
            this.mapping = mapping;
            mappedProperties.AutoMap();
            mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
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
            return mapping.IsSpecified(mappedProperties.Get(property));
        }

        public Access Access
        {
            get { return Access.FromString(mapping.Access); }
        }

        public TypeReference Class
        {
            get { return mapping.Class; }
        }

        public string ForeignKey
        {
            get { return mapping.ForeignKey; }
        }

        public bool LazyLoad
        {
            get { return mapping.Lazy; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public NotFound NotFound
        {
            get { return NotFound.FromString(mapping.NotFound); }
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                return mapping.Columns
                    .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                    .Cast<IColumnInspector>();
            }
        }
    }
}