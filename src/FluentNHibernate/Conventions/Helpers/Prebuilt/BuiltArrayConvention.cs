using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

[Obsolete("Use BuiltCollectionConvention")]
class BuiltArrayConvention(Action<IAcceptanceCriteria<IArrayInspector>> accept, Action<IArrayInstance> convention)
    : BuiltConventionBase<IArrayInspector, IArrayInstance>(accept, convention), IArrayConvention, IArrayConventionAcceptance;
