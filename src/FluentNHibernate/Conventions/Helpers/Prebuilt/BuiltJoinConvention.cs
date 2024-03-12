using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltJoinConvention(Action<IAcceptanceCriteria<IJoinInspector>> accept, Action<IJoinInstance> convention)
    : BuiltConventionBase<IJoinInspector, IJoinInstance>(accept, convention), IJoinConvention, IJoinConventionAcceptance;
