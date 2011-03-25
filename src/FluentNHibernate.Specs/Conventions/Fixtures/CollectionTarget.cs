using System.Collections.Generic;
using FluentNHibernate.Mapping;
using Iesi.Collections.Generic;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class CollectionTarget
    {
        public int Id { get; set; }
        public IList<CollectionChildTarget> Bag { get; set; }
        public ISet<CollectionChildTarget> Set { get; set; }
    }

    public class CollectionChildTarget
    {
        public int Id { get; set; }
    }

    public class CollectionTargetMap : ClassMap<CollectionTarget>
    {
        public CollectionTargetMap()
        {
            Id(x => x.Id);
            HasMany(x => x.Bag);
            HasMany(x => x.Set);
        }
    }

    public class CollectionChildTargetMap : ClassMap<CollectionChildTarget>
    {
        public CollectionChildTargetMap()
        {
            Id(x => x.Id);
        }
    }
}