using System;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    [Multiple]
    internal class BuiltConventionBase<TConventionTarget>
    {
        private readonly Func<TConventionTarget, bool> accept;
        private readonly Action<TConventionTarget> convention;

        public BuiltConventionBase(Func<TConventionTarget, bool> accept, Action<TConventionTarget> convention)
        {
            this.accept = accept;
            this.convention = convention;
        }

        public bool Accept(TConventionTarget target)
        {
            return accept(target);
        }

        public void Apply(TConventionTarget target)
        {
            convention(target);
        }
    }
}