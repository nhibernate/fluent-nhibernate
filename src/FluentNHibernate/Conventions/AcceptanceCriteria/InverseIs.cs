namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class InverseIs
    {
        public IAcceptanceCriterion Set
        {
            get { return new SetCriterion(true); }
        }
    }
}