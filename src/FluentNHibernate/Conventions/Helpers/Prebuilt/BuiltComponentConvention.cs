using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    public class BuiltComponentConvention : BuiltConventionBase<IComponentInspector, IComponentInstance>, IComponentConvention
    {
        public BuiltComponentConvention(Action<IAcceptanceCriteria<IComponentInspector>> accept, Action<IComponentInstance> convention) 
            : base(accept, convention)
        {}
    }
}