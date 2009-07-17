using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class DynamicComponentConventionBuilder : IConventionBuilder<IDynamicComponentConvention, IDynamicComponentInspector, IDynamicComponentInstance>
    {
        public IDynamicComponentConvention Always(Action<IDynamicComponentInstance> convention)
        {
            return new BuiltDynamicComponentConvention(accept => { }, convention);
        }

        public IDynamicComponentConvention When(Action<IAcceptanceCriteria<IDynamicComponentInspector>> expectations, Action<IDynamicComponentInstance> convention)
        {
            return new BuiltDynamicComponentConvention(expectations, convention);
        }
    }
}