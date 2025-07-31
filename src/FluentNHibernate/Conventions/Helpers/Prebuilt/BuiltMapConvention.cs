using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

[Obsolete("Use BuiltCollectionConvention")]
class BuiltMapConvention(Action<IAcceptanceCriteria<IMapInspector>> accept, Action<IMapInstance> convention)
    : BuiltConventionBase<IMapInspector, IMapInstance>(accept, convention), IMapConvention, IMapConventionAcceptance;
