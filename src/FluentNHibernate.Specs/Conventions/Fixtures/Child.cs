using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class Child
    {
        public int Id { get; set; }
        public Parent Parent { get; set; }
    }

    public class ChildMap : ClassMap<Child>
    {
        public ChildMap()
        {
            Id(x => x.Id);
            References(x => x.Parent)
                .Columns("one", "two");
        }
    }
}