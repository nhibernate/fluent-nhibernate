using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

internal class BuiltHasManyConvention(Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> accept, Action<IOneToManyCollectionInstance> convention)
    : BuiltConventionBase<IOneToManyCollectionInspector, IOneToManyCollectionInstance>(accept, convention), IHasManyConvention, IHasManyConventionAcceptance;
