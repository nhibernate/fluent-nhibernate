using Iesi.Collections.Generic;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class SetCompositeElementCollectionEntity
    {
        public int Id { get; set; }
        public ISet<Value> Values { get; set; }
    }
}