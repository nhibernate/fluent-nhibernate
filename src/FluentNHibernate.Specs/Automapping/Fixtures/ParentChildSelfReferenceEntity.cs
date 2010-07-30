using System.Collections.Generic;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    public class ParentChildSelfReferenceEntity
    {
        public int Id { get; set; }
        public ParentChildSelfReferenceEntity Parent { get; set; }
        public IList<ParentChildSelfReferenceEntity> Children { get; set; }
    }
}