using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltDynamicComponentConvention : BuiltConventionBase<IDynamicComponentInspector, IDynamicComponentInstance>, IDynamicComponentConvention
    {
        public BuiltDynamicComponentConvention(Action<IAcceptanceCriteria<IDynamicComponentInspector>> accept, Action<IDynamicComponentInstance> convention) 
            : base(accept, convention)
        {}
    }
}