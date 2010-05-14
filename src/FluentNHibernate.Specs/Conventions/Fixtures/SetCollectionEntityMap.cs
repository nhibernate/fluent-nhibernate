using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class SetCollectionEntityMap : ClassMap<SetCollectionEntity>
    {
        public SetCollectionEntityMap()
        {
            Id(x => x.Id);
            HasMany(x => x.Children);
        }
    }
}