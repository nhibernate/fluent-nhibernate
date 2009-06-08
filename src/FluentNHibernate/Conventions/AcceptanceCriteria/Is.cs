using System;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class Is
    {
        public static IAcceptanceCriterion Set
        {
            get { return new SetCriterion(false); }
        }

        public static InverseIs Not
        {
            get
            {
                return new InverseIs();
            }
        }
    }
}