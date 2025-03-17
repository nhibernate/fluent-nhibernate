using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

public class BuiltCollectionConvention(Action<IAcceptanceCriteria<ICollectionInspector>> accept, Action<ICollectionInstance> convention)
    : BuiltConventionBase<ICollectionInspector, ICollectionInstance>(accept, convention), ICollectionConvention, ICollectionConventionAcceptance;
