using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class ClassConventionBuilder : IConventionBuilder<IClassConvention, IClassMap>
    {
        public IClassConvention Always(Action<IClassMap> convention)
        {
            return new BuiltClassConvention(x => true, convention);
        }

        public IClassConvention When(Func<IClassMap, bool> isTrue, Action<IClassMap> convention)
        {
            return new BuiltClassConvention(isTrue, convention);
        }
    }
}