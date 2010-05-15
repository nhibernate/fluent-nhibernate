using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithCollections
    {
        public EntityCollectionChild[] ArrayOfChildren { get; set; }
        public IList<EntityCollectionChild> BagOfChildren { get; set; }
        public ISet<EntityCollectionChild> SetOfChildren { get; set; }

        public IList<string> BagOfStrings { get; set; }
    }

    class EntityWithFieldCollections
    {
        public IList<EntityCollectionChild> BagOfChildren;
    }

    class EntityCollectionChild
    {
        public int Position { get; set; }
        public AreaComponent Area { get; set; }
    }

    class AreaComponent
    {
        public int Lat { get; set; }
        public int Long { get; set; }
    }
}
