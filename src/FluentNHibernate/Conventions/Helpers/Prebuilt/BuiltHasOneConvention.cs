using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHasOneConvention : BuiltConventionBase<IOneToOneInspector, IOneToOneInstance>, IHasOneConvention
    {
        public BuiltHasOneConvention(Action<IAcceptanceCriteria<IOneToOneInspector>> accept, Action<IOneToOneInstance> convention)
            : base(accept, convention)
        { }
    }
}