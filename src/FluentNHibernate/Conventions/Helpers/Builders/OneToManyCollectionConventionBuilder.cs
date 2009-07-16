using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class OneToManyCollectionConventionBuilder : IConventionBuilder<IHasManyConvention, IOneToManyCollectionInspector, IOneToManyCollectionInstance>
    {
        public IHasManyConvention Always(Action<IOneToManyCollectionInstance> convention)
        {
            return new BuiltOneToManyCollectionConvention(criteria => { }, convention);
        }

        public IHasManyConvention When(Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> expectations, Action<IOneToManyCollectionInstance> convention)
        {
            return new BuiltOneToManyCollectionConvention(expectations, convention);
        }
    }

    public class ManyToManyCollectionConventionBuilder : IConventionBuilder<IHasManyToManyConvention, IManyToManyCollectionInspector, IManyToManyCollectionInstance>
    {
        public IHasManyToManyConvention Always(Action<IManyToManyCollectionInstance> convention)
        {
            return new BuiltManyToManyCollectionConvention(criteria => { }, convention);
        }

        public IHasManyToManyConvention When(Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> expectations, Action<IManyToManyCollectionInstance> convention)
        {
            return new BuiltManyToManyCollectionConvention(expectations, convention);
        }
    }
}