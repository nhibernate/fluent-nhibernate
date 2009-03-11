using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class HasManyConventionBuilder : IConventionBuilder<IHasManyConvention, IOneToManyPart>
    {
        public IHasManyConvention Always(Action<IOneToManyPart> convention)
        {
            return new BuiltHasManyConvention(x => true, convention);
        }

        public IHasManyConvention When(Func<IOneToManyPart, bool> isTrue, Action<IOneToManyPart> convention)
        {
            return new BuiltHasManyConvention(isTrue, convention);
        }
    }
}