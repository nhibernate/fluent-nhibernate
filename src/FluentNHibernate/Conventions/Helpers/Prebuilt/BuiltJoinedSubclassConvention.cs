using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltJoinedSubclassConvention : BuiltConventionBase<IJoinedSubclassInspector, IJoinedSubclassInstance>, IJoinedSubclassConvention
    {
        public BuiltJoinedSubclassConvention(Action<IAcceptanceCriteria<IJoinedSubclassInspector>> accept, Action<IJoinedSubclassInstance> convention)
            : base(accept, convention)
        { }
    }
}