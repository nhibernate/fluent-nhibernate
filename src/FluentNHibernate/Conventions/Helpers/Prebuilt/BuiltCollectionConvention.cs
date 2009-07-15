using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    public class BuiltCollectionConvention : BuiltConventionBase<ICollectionInspector, ICollectionInstance>, ICollectionConvention
    {
        public BuiltCollectionConvention(Action<IAcceptanceCriteria<ICollectionInspector>> accept, Action<ICollectionInstance> convention)
            : base(accept, convention)
        {}
    }
}