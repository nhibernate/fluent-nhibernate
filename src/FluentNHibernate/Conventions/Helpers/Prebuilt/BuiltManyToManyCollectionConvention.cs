using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    public class BuiltManyToManyCollectionConvention : BuiltConventionBase<IManyToManyCollectionInspector, IManyToManyCollectionInstance>, IHasManyToManyConvention
    {
        public BuiltManyToManyCollectionConvention(Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> accept, Action<IManyToManyCollectionInstance> convention)
            : base(accept, convention)
        { }
    }
}