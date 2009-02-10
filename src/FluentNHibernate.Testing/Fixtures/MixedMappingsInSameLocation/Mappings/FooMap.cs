using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.Fixtures.MixedMappingsInSameLocation.Mappings
{
    public class FooMap : ClassMap<Foo>
    {
        public FooMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}