using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class HasManyToManyConventionBuilder : IConventionBuilder<IHasManyToManyConvention, IManyToManyCollectionInspector, IManyToManyCollectionAlteration>
    {
        public IHasManyToManyConvention Always(Action<IManyToManyCollectionAlteration> convention)
        {
            return new BuiltHasManyToManyConvention(accept => { }, (a, i) => convention(a));
        }

        public IHasManyToManyConvention Always(Action<IManyToManyCollectionAlteration, IManyToManyCollectionInspector> convention)
        {
            return new BuiltHasManyToManyConvention(accept => { }, convention);
        }

        public IHasManyToManyConvention When(Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> expectations, Action<IManyToManyCollectionAlteration> convention)
        {
            return new BuiltHasManyToManyConvention(expectations, (a, i) => convention(a));
        }

        public IHasManyToManyConvention When(Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> expectations, Action<IManyToManyCollectionAlteration, IManyToManyCollectionInspector> convention)
        {
            return new BuiltHasManyToManyConvention(expectations, convention);
        }
    }
}