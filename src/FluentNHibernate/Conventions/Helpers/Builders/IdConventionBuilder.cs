using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class IdConventionBuilder : IConventionBuilder<IIdConvention, IIdentityPart>
    {
        public IIdConvention Always(Action<IIdentityPart> convention)
        {
            return new BuiltIdConvention(x => true, convention);
        }

        public IIdConvention When(Func<IIdentityPart, bool> isTrue, Action<IIdentityPart> convention)
        {
            return new BuiltIdConvention(isTrue, convention);
        }
    }
}