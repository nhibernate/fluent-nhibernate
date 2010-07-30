using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    class TwoPropertyEntityMap : ClassMap<TwoPropertyEntity>
    {
        public TwoPropertyEntityMap()
        {
            Id(x => x.Id);
            Map(x => x.TargetProperty);
            Map(x => x.OtherProperty);
        }
    }
}