using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltReferenceConvention(
    Action<IAcceptanceCriteria<IManyToOneInspector>> accept,
    Action<IManyToOneInstance> convention)
    : BuiltConventionBase<IManyToOneInspector, IManyToOneInstance>(accept, convention), IReferenceConvention,
        IReferenceConventionAcceptance;
