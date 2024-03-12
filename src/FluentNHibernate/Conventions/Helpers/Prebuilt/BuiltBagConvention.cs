using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

[Obsolete("Use BuiltCollectionConvention")]
internal class BuiltBagConvention(Action<IAcceptanceCriteria<IBagInspector>> accept, Action<IBagInstance> convention)
    : BuiltConventionBase<IBagInspector, IBagInstance>(accept, convention), IBagConvention, IBagConventionAcceptance;
