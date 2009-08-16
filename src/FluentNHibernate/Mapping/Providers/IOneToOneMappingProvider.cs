using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IOneToOneMappingProvider
    {
        OneToOneMapping GetOneToOneMapping();
    }
}