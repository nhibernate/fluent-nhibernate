using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHasManyToManyConvention : BuiltConventionBase<IManyToManyCollectionInspector, IManyToManyCollectionInstance>, IHasManyToManyConvention
    {
        public BuiltHasManyToManyConvention(Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> accept, Action<IManyToManyCollectionInstance> convention)
            : base(accept, convention)
        { }
    }
}