using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections
{
    public class CompositeIdentityInspector : ICompositeIdentityInspector
    {
        private readonly InspectorModelMapper<ICompositeIdentityInspector, CompositeIdMapping> mappedProperties = new InspectorModelMapper<ICompositeIdentityInspector, CompositeIdMapping>();
        private readonly CompositeIdMapping mapping;

        public CompositeIdentityInspector(CompositeIdMapping mapping)
        {
            this.mapping = mapping;
            mappedProperties.AutoMap();
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

        public IEnumerable<IKeyManyToOneInspector> KeyManyToOnes
        {
            get
            {
                return mapping.KeyManyToOnes
                    .Select(x => new KeyManyToOneInspector(x))
                    .Cast<IKeyManyToOneInspector>();
            }
        }

        public IEnumerable<IKeyPropertyInspector> KeyProperties
        {
            get
            {
                return mapping.KeyProperties
                    .Select(x => new KeyPropertyInspector(x))
                    .Cast<IKeyPropertyInspector>();
            }
        }

        public bool Mapped
        {
            get { return mapping.Mapped; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public string UnsavedValue
        {
            get { return mapping.UnsavedValue; }
        }
    }
}