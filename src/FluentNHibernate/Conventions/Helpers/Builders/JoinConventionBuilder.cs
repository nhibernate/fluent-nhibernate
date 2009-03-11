using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class JoinConventionBuilder : IConventionBuilder<IJoinConvention, IJoin>
    {
        public IJoinConvention Always(Action<IJoin> convention)
        {
            return new BuiltJoinConvention(x => true, convention);
        }

        public IJoinConvention When(Func<IJoin, bool> isTrue, Action<IJoin> convention)
        {
            return new BuiltJoinConvention(isTrue, convention);
        }
    }
}