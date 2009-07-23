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
        }

        public string OrderBy
        {
            get { return mapping.OrderBy; }
        }
    }
}
