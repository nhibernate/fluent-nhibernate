using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.Mapping
{
    public class MappingPartComparer : IComparer<IMappingPart>
    {
        private readonly IMappingPart[] originalPartOrder;

        public MappingPartComparer(IEnumerable<IMappingPart> originalCollection)
        {
            //We create a copy of the collection to maintain the order in which the elements were originally added
            originalPartOrder = originalCollection.ToArray();
        }

        public int Compare(IMappingPart x, IMappingPart y)
        {
            //General Position
            if (x.PositionOnDocument != y.PositionOnDocument) return x.PositionOnDocument.CompareTo(y.PositionOnDocument);
            //Sub-Position if positions are the same
            if (x.LevelWithinPosition != y.LevelWithinPosition) return x.LevelWithinPosition.CompareTo(y.LevelWithinPosition);

            //Relative Index based on the order the part was added
            return Array.IndexOf(originalPartOrder, x).CompareTo(Array.IndexOf(originalPartOrder, y));
        }
    }
}