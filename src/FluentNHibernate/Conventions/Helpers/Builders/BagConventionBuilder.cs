using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class BagConventionBuilder : IConventionBuilder<IBagConvention, IBagInspector, IBagInstance>
    {
        public IBagConvention Always(Action<IBagInstance> convention)
        {
            return new BuiltBagConvention(accept => { }, convention);
        }

        public IBagConvention When(Action<IAcceptanceCriteria<IBagInspector>> expectations, Action<IBagInstance> convention)
        {
            return new BuiltBagConvention(expectations, convention);
        }
    }
}