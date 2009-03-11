using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class HasOneConventionBuilder : IConventionBuilder<IHasOneConvention, IOneToOnePart>
    {
        public IHasOneConvention Always(Action<IOneToOnePart> convention)
        {
            return new BuiltHasOneConvention(x => true, convention);
        }

        public IHasOneConvention When(Func<IOneToOnePart, bool> isTrue, Action<IOneToOnePart> convention)
        {
            return new BuiltHasOneConvention(isTrue, convention);
        }
    }
}