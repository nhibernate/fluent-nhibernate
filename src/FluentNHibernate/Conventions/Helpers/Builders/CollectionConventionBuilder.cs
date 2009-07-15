using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class CollectionConventionBuilder : IConventionBuilder<ICollectionConvention, ICollectionInspector, ICollectionInstance>
    {
        public ICollectionConvention Always(Action<ICollectionInstance> convention)
        {
            return new BuiltCollectionConvention(criteria => { }, convention);
        }

        public ICollectionConvention When(Action<IAcceptanceCriteria<ICollectionInspector>> expectations, Action<ICollectionInstance> convention)
        {
            return new BuiltCollectionConvention(expectations, convention);
        }
    }
}