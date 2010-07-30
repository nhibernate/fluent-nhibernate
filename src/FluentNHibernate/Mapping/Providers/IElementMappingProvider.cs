using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IElementMappingProvider
    {
        ElementMapping GetElementMapping();
    }
}