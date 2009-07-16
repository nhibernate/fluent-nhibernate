using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping
{
    public interface IIdentityMappingProvider
    {
        IdMapping GetIdentityMapping();
    }
}