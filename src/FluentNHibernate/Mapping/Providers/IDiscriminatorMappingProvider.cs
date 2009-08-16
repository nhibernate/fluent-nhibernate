using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IDiscriminatorMappingProvider
    {
        DiscriminatorMapping GetDiscriminatorMapping();
    }
}