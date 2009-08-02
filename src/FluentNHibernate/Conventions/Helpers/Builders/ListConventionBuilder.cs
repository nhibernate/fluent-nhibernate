using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class ListConventionBuilder : IConventionBuilder<IListConvention, IListInspector, IListInstance>
    {
        public IListConvention Always(Action<IListInstance> convention)
        {
            return new BuiltListConvention(accept => { }, convention);
        }

        public IListConvention When(Action<IAcceptanceCriteria<IListInspector>> expectations, Action<IListInstance> convention)
        {
            return new BuiltListConvention(expectations, convention);
        }
    }
}