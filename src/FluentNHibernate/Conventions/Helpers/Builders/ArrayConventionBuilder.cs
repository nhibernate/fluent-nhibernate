using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class ArrayConventionBuilder : IConventionBuilder<IArrayConvention, IArrayInspector, IArrayInstance>
    {
        public IArrayConvention Always(Action<IArrayInstance> convention)
        {
            return new BuiltArrayConvention(accept => { }, convention);
        }

        public IArrayConvention When(Action<IAcceptanceCriteria<IArrayInspector>> expectations, Action<IArrayInstance> convention)
        {
            return new BuiltArrayConvention(expectations, convention);
        }
    }
}