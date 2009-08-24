using System;
using System.Diagnostics;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class Is
    {
        public static IAcceptanceCriterion Set
        {
            get { return new SetCriterion(false); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static InverseIs Not
        {
            get
            {
                return new InverseIs();
            }
        }
    }
}