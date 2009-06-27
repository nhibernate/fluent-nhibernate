using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class PropertyConventionBuilder : IConventionBuilder<IPropertyConvention, IPropertyInspector, IPropertyAlteration, IPropertyInstance>
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