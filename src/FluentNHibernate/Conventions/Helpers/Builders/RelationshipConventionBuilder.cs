using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class RelationshipConventionBuilder : IConventionBuilder<IRelationshipConvention, IRelationship>
    {
        public IRelationshipConvention Always(Action<IRelationship> convention)
        {
            return new BuiltRelationshipConvention(x => true, convention);
        }

        public IRelationshipConvention When(Func<IRelationship, bool> isTrue, Action<IRelationship> convention)
        {
            return new BuiltRelationshipConvention(isTrue, convention);
        }
    }
}