namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class InverseIs
{
    public IAcceptanceCriterion Set => new SetCriterion(true);
}
