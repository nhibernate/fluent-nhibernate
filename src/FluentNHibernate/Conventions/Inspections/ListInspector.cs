using System;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ListInspector : CollectionInspector, IListInspector
    {
        private readonly InspectorModelMapper<IListInspector, ListMapping> mappedProperties = new InspectorModelMapper<IListInspector, ListMapping>();
        private readonly ListMapping mapping;

        public ListInspector(ListMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public IIndexInspectorBase Index
        {
            get
            {
                if (mapping.Index is IndexMapping)
                { return new IndexInspector(mapping.Index as IndexMapping); }

                if (mapping.Index is IndexManyToManyMapping)
                { return new IndexManyToManyInspector(mapping.Index as IndexManyToManyMapping); }

                throw new InvalidOperationException("This IIndexMapping is not a valid type for inspecting");
            }
        }
    }
}
