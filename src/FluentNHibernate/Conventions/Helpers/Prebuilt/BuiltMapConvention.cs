using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    [Obsolete("Use BuiltCollectionConvention")]
    internal class BuiltMapConvention : BuiltConventionBase<IMapInspector, IMapInstance>, IMapConvention, IMapConventionAcceptance
    {
        public BuiltMapConvention(Action<IAcceptanceCriteria<IMapInspector>> accept, Action<IMapInstance> convention)
            : base(accept, convention)
        { }
    }
}
