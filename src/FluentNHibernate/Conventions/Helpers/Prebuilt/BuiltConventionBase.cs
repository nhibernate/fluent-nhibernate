using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    [Multiple]
    internal abstract class BuiltConventionBase<TInspector, TAlteration>
        where TInspector : IInspector
    {
        private readonly Action<IAcceptanceCriteria<TInspector>> accept;
        private readonly Action<TAlteration, TInspector> convention;

        public BuiltConventionBase(Action<IAcceptanceCriteria<TInspector>> accept, Action<TAlteration, TInspector> convention)
        {
            this.accept = accept;
            this.convention = convention;
        }

        public void Accept(IAcceptanceCriteria<TInspector> acceptance)
        {
            accept(acceptance);
        }

        public void Apply(TAlteration alteration, TInspector inspector)
        {
            convention(alteration, inspector);
        }
    }
}