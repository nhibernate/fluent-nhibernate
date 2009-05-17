namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class Is
    {
        public static IAcceptanceCriterion Set
        {
            get { return new SetCriterion(false); }
        }

        public static IAcceptanceCriterion Equal(object value)
        {
            return new EqualCriterion(false, value);
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