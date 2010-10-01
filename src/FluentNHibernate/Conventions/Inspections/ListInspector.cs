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
