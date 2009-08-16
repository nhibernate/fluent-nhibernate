using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Providers
{
    public interface ICompositeElementMappingProvider
    {
        CompositeElementMapping GetCompositeElementMapping();
    }
}