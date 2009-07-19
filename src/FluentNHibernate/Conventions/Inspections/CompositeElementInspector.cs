using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class CompositeElementInspector : ICompositeElementInspector
    {
        private readonly InspectorModelMapper<ICompositeElementInspector, CompositeElementMapping> mappedProperties = new InspectorModelMapper<ICompositeElementInspector, CompositeElementMapping>();
        private readonly CompositeElementMapping mapping;

        public CompositeElementInspector(CompositeElementMapping mapping)
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
            get { return mapping.Class.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(mappedProperties.Get(property));
        }

        public TypeReference Class
        {
            get { return mapping.Class; }
        }

        public IParentInspector Parent
        {
            get
            {
                if (mapping.Parent == null)
                    return new ParentInspector(new ParentMapping());

                return new ParentInspector(mapping.Parent);
            }
        }

        public IEnumerable<IPropertyInspector> Properties
        {
            get
            {
                return mapping.Properties
                    .Select(x => new PropertyInspector(x))
                    .Cast<IPropertyInspector>();
            }
        }

        public IEnumerable<IManyToOneInspector> References
        {
            get
            {
                return mapping.References
                    .Select(x => new ManyToOneInspector(x))
                    .Cast<IManyToOneInspector>();
            }
        }
    }
}