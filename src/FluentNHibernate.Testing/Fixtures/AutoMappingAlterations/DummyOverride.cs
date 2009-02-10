using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations.Model;

namespace FluentNHibernate.Testing.Fixtures.AutoMappingAlterations
{
    public class DummyOverride : IAutoMappingOverride<Baz>
    {
        public void Override(AutoMap<Baz> mapping)
        {
            mapping.SetAttribute("was-overridden", "true");
        }
    }
}