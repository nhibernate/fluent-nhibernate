using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    internal class BuiltIdConvention : IIdConvention
    {
        private readonly Action<IIdentityPart> convention;

        public BuiltIdConvention(Action<IIdentityPart> convention)
        {
            this.convention = convention;
        }

        public bool Accept(IIdentityPart target)
        {
            return true;
        }

        public void Apply(IIdentityPart target)
        {
            convention(target);
        }
    }
}