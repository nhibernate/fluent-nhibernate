using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltBagConvention : BuiltConventionBase<IBagInspector, IBagInstance>, IBagConvention
    {
        public BuiltBagConvention(Action<IAcceptanceCriteria<IBagInspector>> accept, Action<IBagInstance> convention)
            : base(accept, convention)
        { }
    }
}
