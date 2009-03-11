using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class HasManyToManyConventionBuilder : IConventionBuilder<IHasManyToManyConvention, IManyToManyPart>
    {
        public IHasManyToManyConvention Always(Action<IManyToManyPart> convention)
        {
            return new BuiltHasManyToManyConvention(x => true, convention);
        }

        public IHasManyToManyConvention When(Func<IManyToManyPart, bool> isTrue, Action<IManyToManyPart> convention)
        {
            return new BuiltHasManyToManyConvention(isTrue, convention);
        }
    }
}