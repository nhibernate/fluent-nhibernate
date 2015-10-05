using System.Collections.Generic;

namespace FluentNHibernate.Testing.Visitors
{
    public class Holder
    {
        public virtual ICollection<BRankedSecond> ColectionOfBRankedSeconds { get; set; }
    }

    public class BRankedSecond
    {
        public virtual Holder BRankedSecondProperty { get; set; }
    }

    public class ARankedFirst
    {
        public virtual Holder ARankedFirstProperty { get; set; }
    }
}