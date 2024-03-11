using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltHasOneConvention(
    Action<IAcceptanceCriteria<IOneToOneInspector>> accept,
    Action<IOneToOneInstance> convention)
    : BuiltConventionBase<IOneToOneInspector, IOneToOneInstance>(accept, convention), IHasOneConvention,
        IHasOneConventionAcceptance;
