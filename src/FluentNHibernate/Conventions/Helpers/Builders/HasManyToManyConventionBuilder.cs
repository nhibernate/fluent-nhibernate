using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class HasManyToManyConventionBuilder : IConventionBuilder<IHasManyToManyConvention, IManyToManyCollectionInspector, IManyToManyCollectionInstance>
    {
        public IHasManyToManyConvention Always(Action<IManyToManyCollectionInstance> convention)
        {
            return new BuiltHasManyToManyConvention(accept => { }, convention);
        }

        public IHasManyToManyConvention When(Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> expectations, Action<IManyToManyCollectionInstance> convention)
        {
            return new BuiltHasManyToManyConvention(expectations, convention);
        }
    }
}