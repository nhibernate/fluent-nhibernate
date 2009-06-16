using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class HasManyConventionBuilder : IConventionBuilder<IHasManyConvention, IOneToManyCollectionInspector, IOneToManyCollectionAlteration>
    {
        public IHasManyConvention Always(Action<IOneToManyCollectionAlteration> convention)
        {
            return new BuiltHasManyConvention(accept => { }, (a, i) => convention(a));
        }

        public IHasManyConvention Always(Action<IOneToManyCollectionAlteration, IOneToManyCollectionInspector> convention)
        {
            return new BuiltHasManyConvention(accept => { }, convention);
        }

        public IHasManyConvention When(Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> expectations, Action<IOneToManyCollectionAlteration> convention)
        {
            return new BuiltHasManyConvention(expectations, (a, i) => convention(a));
        }

        public IHasManyConvention When(Action<IAcceptanceCriteria<IOneToManyCollectionInspector>> expectations, Action<IOneToManyCollectionAlteration, IOneToManyCollectionInspector> convention)
        {
            return new BuiltHasManyConvention(expectations, convention);
        }
    }
}