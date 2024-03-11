using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

[Obsolete("Use BuiltCollectionConvention")]
internal class BuiltSetConvention(Action<IAcceptanceCriteria<ISetInspector>> accept, Action<ISetInstance> convention)
    : BuiltConventionBase<ISetInspector, ISetInstance>(accept, convention), ISetConvention, ISetConventionAcceptance;
