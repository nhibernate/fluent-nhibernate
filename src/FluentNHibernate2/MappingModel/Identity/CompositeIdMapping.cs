using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Identity
{
    public class CompositeIdMapping : MappingBase<HbmCompositeId>, IIdentityMapping
    {
        public CompositeIdMapping()
        {
            
        }
    }
}