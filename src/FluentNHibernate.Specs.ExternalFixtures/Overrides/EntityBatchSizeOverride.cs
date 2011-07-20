using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace FluentNHibernate.Specs.ExternalFixtures.Overrides
{
    public class EntityBatchSizeOverride : IAutoMappingOverride<Entity>
    {
        public void Override(AutoMapping<Entity> mapping)
        {
            mapping.BatchSize(1234);
        }
    }
}
