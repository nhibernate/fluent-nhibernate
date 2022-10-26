using System.Linq;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmFilterConverter : HbmConverterBase<FilterMapping, HbmFilter>
    {
        private HbmFilter hbmFilter;

        public HbmFilterConverter() : base(null)
        {
        }

        public override HbmFilter Convert(FilterMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmFilter;
        }

        public override void ProcessFilter(FilterMapping filterMapping)
        {
            hbmFilter = new HbmFilter();

            hbmFilter.name = filterMapping.Name;

            if (!string.IsNullOrEmpty(filterMapping.Condition))
                hbmFilter.condition = filterMapping.Condition;
        }
    }
}
