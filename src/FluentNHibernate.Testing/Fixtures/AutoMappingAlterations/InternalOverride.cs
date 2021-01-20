using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations.Model;

namespace FluentNHibernate.Testing.Fixtures.AutoMappingAlterations
{
    internal class InternalOverride : IAutoMappingOverride<Baz>
    {
        public void Override(AutoMapping<Baz> mapping)
        {
            mapping.Table("InternalBaz");
        }
    }
}