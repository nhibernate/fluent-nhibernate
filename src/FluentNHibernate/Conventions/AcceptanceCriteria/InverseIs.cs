namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class InverseIs
    {
        public IAcceptanceCriterion Set
        {
            get { return new SetCriterion(true); }
        }

        public IAcceptanceCriterion Equal(object value)
        {
            return new EqualCriterion(true, value);
        }
    }
}