using Iesi.Collections.Generic;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class SetElementCollectionEntity
    {
        public int Id { get; set; }
        public ISet<string> Strings { get; set; }
    }
}