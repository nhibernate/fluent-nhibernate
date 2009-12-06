using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class KeyPropertyConventionBuilder : IConventionBuilder<IKeyPropertyConvention, IKeyPropertyInspector, IKeyPropertyInstance>
    {
        public IKeyPropertyConvention Always(Action<IKeyPropertyInstance> convention)
        {
            return new BuiltKeyPropertyConvention(accept => { }, convention);
        }

        public IKeyPropertyConvention When(Action<IAcceptanceCriteria<IKeyPropertyInspector>> expectations, Action<IKeyPropertyInstance> convention)
        {
            return new BuiltKeyPropertyConvention(expectations, convention);
        }
    }
}