using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping.Providers;

public interface ISubclassMappingProvider
{
    SubclassMapping GetSubclassMapping();
}
