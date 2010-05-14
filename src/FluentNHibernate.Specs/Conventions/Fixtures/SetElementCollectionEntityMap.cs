using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class SetElementCollectionEntityMap : ClassMap<SetElementCollectionEntity>
    {
        public SetElementCollectionEntityMap()
        {
            Id(x => x.Id);
            HasMany(x => x.Strings)
                .Element("Value");
        }
    }
}