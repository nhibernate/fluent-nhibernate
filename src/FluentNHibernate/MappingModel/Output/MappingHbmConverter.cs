using System.Xml;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class MappingHbmConverter
    {
        public HbmMapping Convert(HibernateMapping mapping)
        {
            return BuildHbm(mapping);
        }

        private static HbmMapping BuildHbm(HibernateMapping rootMapping)
        {
            var hbmConverter = HbmConverterFactory.CreateHibernateMappingConverter();

            return hbmConverter.Convert(rootMapping);
        }
    }
}