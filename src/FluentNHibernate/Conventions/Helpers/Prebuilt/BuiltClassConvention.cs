using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltClassConvention(Action<IAcceptanceCriteria<IClassInspector>> accept, Action<IClassInstance> convention)
    : BuiltConventionBase<IClassInspector, IClassInstance>(accept, convention), IClassConvention, IClassConventionAcceptance;
