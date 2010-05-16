using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Mapping;
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
                return mapping.Keys
                    .Where(x => x is KeyManyToOneMapping)
                    .Select(x => new KeyManyToOneInspector((KeyManyToOneMapping)x))
                    .Cast<IKeyManyToOneInspector>();
            }
        }

        public IEnumerable<IKeyPropertyInspector> KeyProperties
        {
            get
            {
                return mapping.Keys
                    .Where(x => x is KeyPropertyMapping)
                    .Select(x => new KeyPropertyInspector((KeyPropertyMapping)x))
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