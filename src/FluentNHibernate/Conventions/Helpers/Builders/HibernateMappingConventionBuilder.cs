using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class HibernateMappingConventionBuilder : IConventionBuilder<IHibernateMappingConvention, IHibernateMappingInspector, IHibernateMappingInstance>
    {
        public IHibernateMappingConvention Always(Action<IHibernateMappingInstance> convention)
        {
            return new BuiltHibernateMappingConvention(accept => { }, convention);
        }

        public IHibernateMappingConvention When(Action<IAcceptanceCriteria<IHibernateMappingInspector>> expectations, Action<IHibernateMappingInstance> convention)
        {
            return new BuiltHibernateMappingConvention(expectations, convention);
        }
    }
}