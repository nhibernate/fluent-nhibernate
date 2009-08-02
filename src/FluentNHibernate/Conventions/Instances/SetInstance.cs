using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class SetInstance : SetInspector, ISetInstance
    {
        private readonly SetMapping mapping;
        public SetInstance(SetMapping mapping)
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

        public void SetSort(string sort)
        {
            if (mapping.IsSpecified(x => x.Sort))
                return;

            mapping.Sort = sort;
        }
    }
}
