using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IManyToOneMappingProvider
    {
        ManyToOneMapping GetManyToOneMapping();
    }
}