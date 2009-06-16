using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class PropertyConventionBuilder : IConventionBuilder<IPropertyConvention, IPropertyInspector, IPropertyAlteration>
    {
        public IPropertyConvention Always(Action<IPropertyAlteration> convention)
        {
            return new BuiltPropertyConvention(accept => { }, (a, i) => convention(a));
        }

        public IPropertyConvention Always(Action<IPropertyAlteration, IPropertyInspector> convention)
        {
            return new BuiltPropertyConvention(accept => { }, convention);
        }

        public IPropertyConvention When(Action<IAcceptanceCriteria<IPropertyInspector>> expectations, Action<IPropertyAlteration> convention)
        {
            return new BuiltPropertyConvention(expectations, (a, i) => convention(a));
        }

        public IPropertyConvention When(Action<IAcceptanceCriteria<IPropertyInspector>> expectations, Action<IPropertyAlteration, IPropertyInspector> convention)
        {
            return new BuiltPropertyConvention(expectations, convention);
        }
    }
}