using System.Collections.Generic;
using FluentNHibernate.Specs.ExternalFixtures;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    class ReadOnlyEnumerableEntity
    {
        IList<EntityChild> backingFieldCollection;

        public int Id { get; set; }
        public IEnumerable<EntityChild> AutoPropertyCollection { get; private set; }
        public IEnumerable<EntityChild> BackingFieldCollection
        {
            get { return backingFieldCollection; }
        }
    }
}