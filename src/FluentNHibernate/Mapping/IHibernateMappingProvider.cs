using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IHibernateMappingProvider
    {
        HibernateMapping GetHibernateMapping();
    }
}