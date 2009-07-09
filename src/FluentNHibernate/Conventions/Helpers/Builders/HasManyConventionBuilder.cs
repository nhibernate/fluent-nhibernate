using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class HasManyConventionBuilder : IConventionBuilder<IHasManyConvention, IOneToManyCollectionInspector, IOneToManyCollectionInstance>
    {
        public IHasManyConvention Always(Action<IOneToManyCollectionInstance> convention)
        {
            return new BuiltHasManyConvention(accept => { }, convention);
        }

        public IHasManyConvention When(Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> expectations, Action<IOneToManyCollectionInstance> convention)
        {
            return new BuiltHasManyConvention(expectations, convention);
        }
    }
}