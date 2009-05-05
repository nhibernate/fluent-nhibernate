using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltRelationshipConvention : BuiltConventionBase<IRelationship>, IRelationshipConvention
    {
        public BuiltRelationshipConvention(Func<IRelationship, bool> accept, Action<IRelationship> convention)
            : base(accept, convention)
        { }
    }
}