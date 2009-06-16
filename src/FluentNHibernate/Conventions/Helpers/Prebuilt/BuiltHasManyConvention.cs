using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHasManyConvention : BuiltConventionBase<IOneToManyCollectionInspector, IOneToManyCollectionAlteration>, IHasManyConvention
    {
        public BuiltHasManyConvention(Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> accept, Action<IOneToManyCollectionAlteration, IOneToManyCollectionInspector> convention)
            : base(accept, convention)
        { }
    }
}