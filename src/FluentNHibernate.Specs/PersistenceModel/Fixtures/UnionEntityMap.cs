using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.PersistenceModel.Fixtures
{
    class UnionEntityMap : ClassMap<UnionEntity>
    {
        public UnionEntityMap()
        {
            Id(x => x.Id);
            UseUnionSubclassForInheritanceMapping();
        }
    }
}