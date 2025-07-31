using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping;

public interface IFilterDefinition
{
    string Name { get; }
    FilterDefinitionMapping GetFilterMapping();
    HibernateMapping GetHibernateMapping();
}
