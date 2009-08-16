using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class JoinConventionBuilder : IConventionBuilder<IJoinConvention, IJoinInspector, IJoinInstance>
    {
        public IJoinConvention Always(Action<IJoinInstance> convention)
        {
            return new BuiltJoinConvention(accept => { }, convention);
        }

        public IJoinConvention When(Action<IAcceptanceCriteria<IJoinInspector>> expectations, Action<IJoinInstance> convention)
        {
            return new BuiltJoinConvention(expectations, convention);
        }
    }
}