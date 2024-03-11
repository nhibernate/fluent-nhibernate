using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

public class BuiltManyToManyCollectionConvention(
    Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> accept,
    Action<IManyToManyCollectionInstance> convention)
    : BuiltConventionBase<IManyToManyCollectionInspector, IManyToManyCollectionInstance>(accept, convention),
        IHasManyToManyConvention, IHasManyToManyConventionAcceptance;
