using System;
using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class MapInspector : CollectionInspector, IMapInspector
    {
        private readonly InspectorModelMapper<IMapInspector, MapMapping> mappedProperties = new InspectorModelMapper<IMapInspector, MapMapping>();
        private readonly MapMapping mapping;

        public MapInspector(MapMapping mapping)
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
        public new string OrderBy
        {
            get { return mapping.OrderBy; }
        }
        public string Sort
        {
            get { return mapping.Sort; }
        }
    }
}
