using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

class BuiltJoinedSubclassConvention(Action<IAcceptanceCriteria<IJoinedSubclassInspector>> accept, Action<IJoinedSubclassInstance> convention)
    : BuiltConventionBase<IJoinedSubclassInspector, IJoinedSubclassInstance>(accept, convention), IJoinedSubclassConvention, IJoinedSubclassConventionAcceptance;
