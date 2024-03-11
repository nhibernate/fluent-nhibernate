using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

public class BuiltOneToManyCollectionConvention(
    Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> accept,
    Action<IOneToManyCollectionInstance> convention)
    : BuiltConventionBase<IOneToManyCollectionInspector, IOneToManyCollectionInstance>(accept, convention),
        IHasManyConvention, IHasManyConventionAcceptance;
