using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class FilterTarget
    {
        public int Id { get; set; }
        public IList<FilterChildTarget> ChildOneToMany { get; set; }
        public IList<FilterChildTarget> ChildManyToMany { get; set; }
    }

    public class FilterChildTarget
    {
        public int Id { get; set; }
    }

    public class FilterTargetMap : ClassMap<FilterTarget>
    {
        public FilterTargetMap()
        {
            Id(x => x.Id);
            HasMany(x => x.ChildOneToMany);
            HasManyToMany(x => x.ChildManyToMany);
        }
    }

    public class FilterChildTargetMap : ClassMap<FilterChildTarget>
    {
        public FilterChildTargetMap()
        {
            Id(x => x.Id);
        }
    }
}