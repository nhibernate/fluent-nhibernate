﻿using System;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ArrayInspector : CollectionInspector, IArrayInspector
    {
        private readonly InspectorModelMapper<IArrayInspector, ArrayMapping> mappedProperties = new InspectorModelMapper<IArrayInspector, ArrayMapping>();
        private readonly ArrayMapping mapping;

        public ArrayInspector(ArrayMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
            mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
        }

        public new bool IsSet(Member property)
        {
            return mapping.IsSpecified(mappedProperties.Get(property));
        }

        public IIndexInspectorBase Index
        {
            get
            {
                if (mapping.Index == null)
                    return new IndexInspector(new IndexMapping());

                var index = mapping.Index as IndexMapping;

                if (index != null)
                {
                    if (index.IsManyToMany)
                        return new IndexManyToManyInspector(index);

                    return new IndexInspector(index);
                }

                throw new InvalidOperationException("This IIndexMapping is not a valid type for inspecting");
            }
        }
    }
}
