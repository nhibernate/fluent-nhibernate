using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class ClassConventionBuilder : IConventionBuilder<IClassConvention, IClassInspector, IClassAlteration, IClassInstance>
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