using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    internal class BuiltClassConvention : IClassConvention
    {
        private readonly Func<IClassMap, bool> accept;
        private readonly Action<IClassMap> convention;

        public BuiltClassConvention(Func<IClassMap, bool> accept, Action<IClassMap> convention)
        {
            this.accept = accept;
            this.convention = convention;
        }

        public bool Accept(IClassMap target)
        {
            return accept(target);
        }

        public void Apply(IClassMap target)
        {
            convention(target);
        }
    }
}