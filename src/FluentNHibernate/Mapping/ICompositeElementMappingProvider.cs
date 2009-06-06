using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public interface ICompositeElementMappingProvider
    {
        CompositeElementMapping GetCompositeElementMapping();
    }
}