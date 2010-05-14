using Iesi.Collections.Generic;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class SetCollectionEntity
    {
        public int Id { get; set; }
        public ISet<Child> Children { get; set; }
    }
}