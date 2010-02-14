using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class KeyManyToOneConventionBuilder : IConventionBuilder<IKeyManyToOneConvention, IKeyManyToOneInspector, IKeyManyToOneInstance>
    {
        public IKeyManyToOneConvention Always(Action<IKeyManyToOneInstance> convention)
        {
            return new BuiltKeyManyToOneConvention(accept => { }, convention);
        }

        public IKeyManyToOneConvention When(Action<IAcceptanceCriteria<IKeyManyToOneInspector>> expectations, Action<IKeyManyToOneInstance> convention)
        {
            return new BuiltKeyManyToOneConvention(expectations, convention);
        }
    }
}