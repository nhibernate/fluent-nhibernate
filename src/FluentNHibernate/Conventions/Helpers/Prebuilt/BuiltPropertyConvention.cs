using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

class BuiltPropertyConvention(Action<IAcceptanceCriteria<IPropertyInspector>> accept, Action<IPropertyInstance> convention)
    : BuiltConventionBase<IPropertyInspector, IPropertyInstance>(accept, convention), IPropertyConvention, IPropertyConventionAcceptance;
