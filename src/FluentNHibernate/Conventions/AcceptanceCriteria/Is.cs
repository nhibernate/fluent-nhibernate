using System.Diagnostics;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class Is
{
    public static IAcceptanceCriterion Set => new SetCriterion(false);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public static InverseIs Not => new();
}
