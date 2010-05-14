using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class Parent
    {
        public int Id { get; set; }
        public IList<Child> Children { get; set; }
    }

    public class ParentMap : ClassMap<Parent>
    {
        public ParentMap()
        {
            Id(x => x.Id);
            HasMany(x => x.Children);
        }
    }
}