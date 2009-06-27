using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHasManyConvention : BuiltConventionBase<IOneToManyCollectionInspector, IOneToManyCollectionInstance>, IHasManyConvention
    {
        public BuiltHasManyConvention(Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> accept, Action<IOneToManyCollectionInstance> convention)
            : base(accept, convention)
        { }
    }
}