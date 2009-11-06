using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IStoredProcedureMappingProvider
    {
        StoredProcedureMapping GetStoredProcedureMapping();
    }
}
