using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltKeyPropertyConvention(Action<IAcceptanceCriteria<IKeyPropertyInspector>> accept, Action<IKeyPropertyInstance> convention)
    : BuiltConventionBase<IKeyPropertyInspector, IKeyPropertyInstance>(accept, convention), IKeyPropertyConvention, IKeyPropertyConventionAcceptance;
