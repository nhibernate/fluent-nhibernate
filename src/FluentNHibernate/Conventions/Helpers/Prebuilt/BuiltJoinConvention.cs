using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltJoinConvention : BuiltConventionBase<IJoinInspector, IJoinInstance>, IJoinConvention
    {
        public BuiltJoinConvention(Action<IAcceptanceCriteria<IJoinInspector>> accept, Action<IJoinInstance> convention)
            : base(accept, convention)
        { }
    }
}