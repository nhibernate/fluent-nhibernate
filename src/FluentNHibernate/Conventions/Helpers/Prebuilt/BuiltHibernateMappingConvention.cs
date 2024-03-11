using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltHibernateMappingConvention(
    Action<IAcceptanceCriteria<IHibernateMappingInspector>> accept,
    Action<IHibernateMappingInstance> convention)
    : BuiltConventionBase<IHibernateMappingInspector, IHibernateMappingInstance>(accept, convention),
        IHibernateMappingConvention;
