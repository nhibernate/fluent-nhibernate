using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class BagInspector : CollectionInspector, IBagInspector
    {
        private readonly InspectorModelMapper<IBagInspector, BagMapping> mappedProperties = new InspectorModelMapper<IBagInspector, BagMapping>();
        private readonly BagMapping mapping;

        public BagInspector(BagMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
            mappedProperties.AutoMap();
            mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
        }

        public new bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(mappedProperties.Get(property));
        }

        public string OrderBy
        {
            get { return mapping.OrderBy; }
        }
    }
}
