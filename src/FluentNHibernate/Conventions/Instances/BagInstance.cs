using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class BagInstance : BagInspector, IBagInstance
    {
        private readonly BagMapping mapping;
        public BagInstance(BagMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public void SetOrderBy(string orderBy)
        {
            if (mapping.IsSpecified(x => x.OrderBy))
                return;

            mapping.OrderBy = orderBy;
        }
    }
}
