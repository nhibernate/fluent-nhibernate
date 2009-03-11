using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class DynamicComponentConventionBuilder : IConventionBuilder<IDynamicComponentConvention, IDynamicComponent>
    {
        public IDynamicComponentConvention Always(Action<IDynamicComponent> convention)
        {
            return new BuiltDynamicComponentConvention(x => true, convention);
        }

        public IDynamicComponentConvention When(Func<IDynamicComponent, bool> isTrue, Action<IDynamicComponent> convention)
        {
            return new BuiltDynamicComponentConvention(isTrue, convention);
        }
    }
}