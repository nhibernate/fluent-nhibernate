using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class SetConventionBuilder : IConventionBuilder<ISetConvention, ISetInspector, ISetInstance>
    {
        public ISetConvention Always(Action<ISetInstance> convention)
        {
            return new BuiltSetConvention(accept => { }, convention);
        }

        public ISetConvention When(Action<IAcceptanceCriteria<ISetInspector>> expectations, Action<ISetInstance> convention)
        {
            return new BuiltSetConvention(expectations, convention);
        }
    }
}