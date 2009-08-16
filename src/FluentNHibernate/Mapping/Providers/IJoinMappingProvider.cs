using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IJoinMappingProvider
    {
        JoinMapping GetJoinMapping();
    }
}