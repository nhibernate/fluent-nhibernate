using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class JoinedSubclassConventionBuilder : IConventionBuilder<IJoinedSubclassConvention, IJoinedSubclassInspector, IJoinedSubclassInstance>
    {
        public IJoinedSubclassConvention Always(Action<IJoinedSubclassInstance> convention)
        {
            return new BuiltJoinedSubclassConvention(accept => { }, convention);
        }

        public IJoinedSubclassConvention When(Action<IAcceptanceCriteria<IJoinedSubclassInspector>> expectations, Action<IJoinedSubclassInstance> convention)
        {
            return new BuiltJoinedSubclassConvention(expectations, convention);
        }
    }
}