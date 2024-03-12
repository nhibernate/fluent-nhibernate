using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltCompositeIdConvention(Action<IAcceptanceCriteria<ICompositeIdentityInspector>> accept, Action<ICompositeIdentityInstance> convention)
    : BuiltConventionBase<ICompositeIdentityInspector, ICompositeIdentityInstance>(accept, convention), ICompositeIdentityConvention, ICompositeIdentityConventionAcceptance;
