using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltHasOneConvention : BuiltConventionBase<IOneToOneInspector, IOneToOneInstance>, IHasOneConvention, IHasOneConventionAcceptance
{
    public BuiltHasOneConvention(Action<IAcceptanceCriteria<IOneToOneInspector>> accept, Action<IOneToOneInstance> convention)
        : base(accept, convention)
    { }
}
