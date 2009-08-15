using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations.Model;

namespace FluentNHibernate.Testing.Fixtures.AutoMappingAlterations
{
    public class DummyOverride : IAutoMappingOverride<Baz>
    {
        public void Override(AutoMapping<Baz> mapping)
        {
            mapping.BatchSize(10);
        }
    }
}