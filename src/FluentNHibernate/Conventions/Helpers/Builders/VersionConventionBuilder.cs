using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class VersionConventionBuilder : IConventionBuilder<IVersionConvention, IVersion>
    {
        public IVersionConvention Always(Action<IVersion> convention)
        {
            return new BuiltVersionConvention(x => true, convention);
        }

        public IVersionConvention When(Func<IVersion, bool> isTrue, Action<IVersion> convention)
        {
            return new BuiltVersionConvention(isTrue, convention);
        }
    }
}