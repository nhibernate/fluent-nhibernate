using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class StoredProcedurePart : IStoredProcedureMappingProvider
    {
        readonly StoredProcedureMapping mapping = new StoredProcedureMapping();

        public StoredProcedurePart(string element, string innerText)
        {
            mapping.Set(x => x.SPType, Layer.Defaults, element);
            mapping.Set(x => x.Query, Layer.Defaults, innerText);
            mapping.Set(x => x.Check, Layer.Defaults, "rowcount");
        }

        public CheckTypeExpression<StoredProcedurePart> Check
        {
            get { return new CheckTypeExpression<StoredProcedurePart>(this, value => mapping.Set(x => x.Check, Layer.UserSupplied, value)); }
        }

        StoredProcedureMapping IStoredProcedureMappingProvider.GetStoredProcedureMapping()
        {
            return mapping;
        }
    }
}
