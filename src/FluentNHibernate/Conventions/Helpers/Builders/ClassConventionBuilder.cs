using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class ClassConventionBuilder : IConventionBuilder<IClassConvention, IClassInspector, IClassInstance>
    {
        public IClassConvention Always(Action<IClassInstance> convention)
        {
            return new BuiltClassConvention(accept => { }, convention);
        }

        public IClassConvention When(Action<IAcceptanceCriteria<IClassInspector>> expectations, Action<IClassInstance> convention)
        {
            return new BuiltClassConvention(expectations, convention);
        }
    }
}