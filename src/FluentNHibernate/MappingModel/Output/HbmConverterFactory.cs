using FluentNHibernate.Infrastructure;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public static class HbmConverterFactory
    {
        private static readonly Container Container = new HbmConverterContainer();

        public static IHbmConverter<HibernateMapping, HbmMapping> CreateHibernateMappingConverter()
        {
            return Container.Resolve<IHbmConverter<HibernateMapping, HbmMapping>>();
        }
    }
}
