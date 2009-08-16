using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IIdentityMappingProvider
    {
        IdMapping GetIdentityMapping();
    }
}