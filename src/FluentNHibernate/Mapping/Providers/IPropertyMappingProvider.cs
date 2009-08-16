using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IPropertyMappingProvider
    {
        PropertyMapping GetPropertyMapping();
    }
}