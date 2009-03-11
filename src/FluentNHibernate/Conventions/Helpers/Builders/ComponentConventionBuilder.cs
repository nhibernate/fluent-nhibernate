using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class ComponentConventionBuilder : IConventionBuilder<IComponentConvention, IComponent>
    {
        public IComponentConvention Always(Action<IComponent> convention)
        {
            return new BuiltComponentConvention(x => true, convention);
        }

        public IComponentConvention When(Func<IComponent, bool> isTrue, Action<IComponent> convention)
        {
            return new BuiltComponentConvention(isTrue, convention);
        }
    }
}