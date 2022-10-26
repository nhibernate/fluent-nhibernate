using System.Linq;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmFilterDefinitionConverter : HbmConverterBase<FilterDefinitionMapping, HbmFilterDef>
    {
        private HbmFilterDef hbmFilterDef;

        public HbmFilterDefinitionConverter() : base(null)
        {
        }

        public override HbmFilterDef Convert(FilterDefinitionMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmFilterDef;
        }

        public override void ProcessFilterDefinition(FilterDefinitionMapping filterDefinitionMapping)
        {
            hbmFilterDef = new HbmFilterDef();

            hbmFilterDef.name = filterDefinitionMapping.Name;

            if (!string.IsNullOrEmpty(filterDefinitionMapping.Condition))
                hbmFilterDef.condition = filterDefinitionMapping.Condition;

            if (filterDefinitionMapping.Parameters.Any())
                hbmFilterDef.Items = filterDefinitionMapping.Parameters.Select(paramPair => paramPair.ToHbmFilterParam()).ToArray();
        }
    }
}
