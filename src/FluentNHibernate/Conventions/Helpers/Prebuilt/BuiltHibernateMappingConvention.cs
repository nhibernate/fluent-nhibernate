using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltHibernateMappingConvention : BuiltConventionBase<IHibernateMappingInspector, IHibernateMappingInstance>, IHibernateMappingConvention
    {
        public BuiltHibernateMappingConvention(Action<IAcceptanceCriteria<IHibernateMappingInspector>> accept, Action<IHibernateMappingInstance> convention)
            : base(accept, convention)
        {}
    }
}