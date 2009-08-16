using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class SubclassConventionBuilder : IConventionBuilder<ISubclassConvention, ISubclassInspector, ISubclassInstance>
    {
        public ISubclassConvention Always(Action<ISubclassInstance> convention)
        {
            return new BuiltSubclassConvention(accept => { }, convention);
        }

        public ISubclassConvention When(Action<IAcceptanceCriteria<ISubclassInspector>> expectations, Action<ISubclassInstance> convention)
        {
            return new BuiltSubclassConvention(expectations, convention);
        }
    }
}