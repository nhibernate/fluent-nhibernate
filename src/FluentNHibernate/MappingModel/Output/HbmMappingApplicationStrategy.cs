using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    /// <summary>
    /// A mapping application strategy which utilizes <see cref="MappingHbmConverter"/> for translation.
    /// </summary>
    public class HbmMappingApplicationStrategy : MappingApplicationStrategyBase<HbmMapping>
    {
        public HbmMappingApplicationStrategy()
        {
        }

        protected override HbmMapping ToIntermediateForm(HibernateMapping mapping)
        {
            return new MappingHbmConverter().Convert(mapping);
        }

        protected override void ApplyIntermediateFormToConfiguration(HbmMapping intermediateForm, Configuration cfg)
        {
            cfg.AddDeserializedMapping(intermediateForm, "(XmlDocument)");
        }
    }
}
