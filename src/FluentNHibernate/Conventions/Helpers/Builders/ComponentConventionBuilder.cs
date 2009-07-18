using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class ComponentConventionBuilder : IConventionBuilder<IComponentConvention, IComponentInspector, IComponentInstance>
    {
        public IComponentConvention Always(Action<IComponentInstance> convention)
        {
            return new BuiltComponentConvention(accept => { }, convention);
        }

        public IComponentConvention When(Action<IAcceptanceCriteria<IComponentInspector>> expectations, Action<IComponentInstance> convention)
        {
            return new BuiltComponentConvention(expectations, convention);
        }
    }
}