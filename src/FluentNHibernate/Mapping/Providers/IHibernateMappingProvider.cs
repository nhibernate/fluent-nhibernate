using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IHibernateMappingProvider
    {
        HibernateMapping GetHibernateMapping();
    }
}