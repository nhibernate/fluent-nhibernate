using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class HasOneConventionBuilder : IConventionBuilder<IHasOneConvention, IOneToOneInspector, IOneToOneInstance>
    {
        public IHasOneConvention Always(Action<IOneToOneInstance> convention)
        {
            return new BuiltHasOneConvention(x => { }, convention);
        }

        public IHasOneConvention When(Action<IAcceptanceCriteria<IOneToOneInspector>> expectations, Action<IOneToOneInstance> convention)
        {
            return new BuiltHasOneConvention(expectations, convention);
        }
    }
}