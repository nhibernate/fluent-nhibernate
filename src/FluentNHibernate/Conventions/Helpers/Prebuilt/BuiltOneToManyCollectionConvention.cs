using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    public class BuiltOneToManyCollectionConvention : BuiltConventionBase<IOneToManyCollectionInspector, IOneToManyCollectionInstance>, IHasManyConvention
    {
        public BuiltOneToManyCollectionConvention(Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> accept, Action<IOneToManyCollectionInstance> convention)
            : base(accept, convention)
        { }
    }
}