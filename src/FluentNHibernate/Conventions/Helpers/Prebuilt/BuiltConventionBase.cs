using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    [Multiple]
    public abstract class BuiltConventionBase<TInspector, TInstance>
        where TInspector : IInspector
    {
        private readonly Action<IAcceptanceCriteria<TInspector>> accept;
        private readonly Action<TInstance> convention;

        public BuiltConventionBase(Action<IAcceptanceCriteria<TInspector>> accept, Action<TInstance> convention)
        {
            this.accept = accept;
            this.convention = convention;
        }

        public void Accept(IAcceptanceCriteria<TInspector> acceptance)
        {
            accept(acceptance);
        }

        public void Apply(TInstance instance)
        {
            convention(instance);
        }
    }
}