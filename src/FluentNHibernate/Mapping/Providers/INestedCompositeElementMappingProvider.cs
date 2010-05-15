using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Providers
{
    public interface INestedCompositeElementMappingProvider
    {
        NestedCompositeElementMapping GetCompositeElementMapping();
    }
}