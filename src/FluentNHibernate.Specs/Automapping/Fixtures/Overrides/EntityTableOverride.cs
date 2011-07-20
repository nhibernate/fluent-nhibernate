using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Specs.ExternalFixtures;

namespace FluentNHibernate.Specs.Automapping.Fixtures.Overrides
{
    public class EntityTableOverride : IAutoMappingOverride<Entity>
    {
        public void Override(AutoMapping<Entity> mapping)
        {
            mapping.Table("OverriddenTableName");
        }
    }
}