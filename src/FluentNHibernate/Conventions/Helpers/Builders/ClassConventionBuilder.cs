using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class ClassConventionBuilder : IConventionBuilder<IClassConvention, IClassInspector, IClassAlteration>
    {
        public IClassConvention Always(Action<IClassAlteration> convention)
        {
            return new BuiltClassConvention(accept => { }, (a, i) => convention(a));
        }

        public IClassConvention Always(Action<IClassAlteration, IClassInspector> convention)
        {
            return new BuiltClassConvention(accept => { }, convention);
        }

        public IClassConvention When(Action<IAcceptanceCriteria<IClassInspector>> expectations, Action<IClassAlteration> convention)
        {
            return new BuiltClassConvention(expectations, (a, i) => convention(a));
        }

        public IClassConvention When(Action<IAcceptanceCriteria<IClassInspector>> expectations, Action<IClassAlteration, IClassInspector> convention)
        {
            return new BuiltClassConvention(expectations, convention);
        }
    }
}