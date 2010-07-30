using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class SetCompositeElementCollectionEntityMap : ClassMap<SetCompositeElementCollectionEntity>
    {
        public SetCompositeElementCollectionEntityMap()
        {
            Id(x => x.Id);
            HasMany(x => x.Values)
                .Component(c => c.Map(x => x.Property));
        }
    }
}