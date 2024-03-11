using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltSubclassConvention(
    Action<IAcceptanceCriteria<ISubclassInspector>> accept,
    Action<ISubclassInstance> convention)
    : BuiltConventionBase<ISubclassInspector, ISubclassInstance>(accept, convention), ISubclassConvention,
        ISubclassConventionAcceptance;
