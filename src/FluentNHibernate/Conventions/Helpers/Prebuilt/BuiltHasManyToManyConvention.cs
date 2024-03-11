using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltHasManyToManyConvention(
    Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> accept,
    Action<IManyToManyCollectionInstance> convention)
    : BuiltConventionBase<IManyToManyCollectionInspector, IManyToManyCollectionInstance>(accept, convention),
        IHasManyToManyConvention, IHasManyToManyConventionAcceptance;
