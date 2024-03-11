using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

public class BuiltComponentConvention(
    Action<IAcceptanceCriteria<IComponentInspector>> accept,
    Action<IComponentInstance> convention)
    : BuiltConventionBase<IComponentInspector, IComponentInstance>(accept, convention), IComponentConvention,
        IComponentConventionAcceptance;
