using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class PropertyConventionBuilder : IConventionBuilder<IPropertyConvention, IPropertyInspector, IPropertyInstance>
    {
        public IPropertyConvention Always(Action<IPropertyInstance> convention)
        {
            return new BuiltPropertyConvention(accept => { }, convention);
        }

        public IPropertyConvention When(Action<IAcceptanceCriteria<IPropertyInspector>> expectations, Action<IPropertyInstance> convention)
        {
            return new BuiltPropertyConvention(expectations, convention);
        }
    }
}