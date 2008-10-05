using System.Collections.Generic;

namespace FluentNHibernate.Mapping
{
    public class MappingPartComparer : IComparer<IMappingPart>
    {
        public int Compare(IMappingPart x, IMappingPart y)
        {
            // this isn't exactly nice, but it works (and it's covered by tests... hint hint)
            if (x.Position == PartPosition.First && y.Position != PartPosition.First) return -1;
            if (x.Position == PartPosition.Last && y.Position != PartPosition.Last) return 1;
            if (x.Position == PartPosition.Anywhere && y.Position == PartPosition.First) return 1;
            if (x.Position == PartPosition.Anywhere && y.Position == PartPosition.Last) return -1;

            return x.Level.CompareTo(y.Level);
        }
    }
}