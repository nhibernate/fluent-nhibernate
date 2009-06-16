using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHasManyToManyConvention : BuiltConventionBase<IManyToManyCollectionInspector, IManyToManyCollectionAlteration>, IHasManyToManyConvention
    {
        public BuiltHasManyToManyConvention(Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> accept, Action<IManyToManyCollectionAlteration, IManyToManyCollectionInspector> convention)
            : base(accept, convention)
        { }
    }
}