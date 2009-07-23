using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class SetInspector : CollectionInspector, ISetInspector
    {
        private readonly InspectorModelMapper<ISetInspector, SetMapping> mappedProperties = new InspectorModelMapper<ISetInspector, SetMapping>();
        private readonly SetMapping mapping;

        public SetInspector(SetMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public string OrderBy
        {
            get { return mapping.OrderBy; }
        }
        public string Sort
        {
            get { return mapping.Sort; }
        }
    }
}
