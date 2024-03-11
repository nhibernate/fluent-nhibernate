using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

public class BuiltDynamicComponentConvention(
    Action<IAcceptanceCriteria<IDynamicComponentInspector>> accept,
    Action<IDynamicComponentInstance> convention)
    : BuiltConventionBase<IDynamicComponentInspector, IDynamicComponentInstance>(accept, convention),
        IDynamicComponentConvention, IDynamicComponentConventionAcceptance;
