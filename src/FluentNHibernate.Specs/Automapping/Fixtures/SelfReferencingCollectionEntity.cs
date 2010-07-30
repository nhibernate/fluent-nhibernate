using System.Collections.Generic;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    public class SelfReferencingCollectionEntity
    {
        public int Id { get; set; }
        public IList<SelfReferencingCollectionEntity> Children { get; set; }
    }
}