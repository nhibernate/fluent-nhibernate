using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

[Obsolete("Use BuiltCollectionConvention")]
class BuiltListConvention(Action<IAcceptanceCriteria<IListInspector>> accept, Action<IListInstance> convention)
    : BuiltConventionBase<IListInspector, IListInstance>(accept, convention), IListConvention, IListConventionAcceptance;
