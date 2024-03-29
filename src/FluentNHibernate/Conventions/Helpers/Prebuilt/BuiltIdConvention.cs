using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

class BuiltIdConvention(Action<IAcceptanceCriteria<IIdentityInspector>> accept, Action<IIdentityInstance> convention)
    : BuiltConventionBase<IIdentityInspector, IIdentityInstance>(accept, convention), IIdConvention, IIdConventionAcceptance;
