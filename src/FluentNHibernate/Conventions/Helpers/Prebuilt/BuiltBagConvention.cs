using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    [Obsolete("Use BuiltCollectionConvention")]
    internal class BuiltBagConvention : BuiltConventionBase<IBagInspector, IBagInstance>, IBagConvention, IBagConventionAcceptance
    {
        public BuiltBagConvention(Action<IAcceptanceCriteria<IBagInspector>> accept, Action<IBagInstance> convention)
            : base(accept, convention)
        { }
    }
}
