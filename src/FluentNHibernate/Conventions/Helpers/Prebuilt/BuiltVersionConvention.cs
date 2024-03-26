using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

class BuiltVersionConvention(Action<IAcceptanceCriteria<IVersionInspector>> accept, Action<IVersionInstance> convention)
    : BuiltConventionBase<IVersionInspector, IVersionInstance>(accept, convention), IVersionConvention, IVersionConventionAcceptance;
