using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltPropertyConvention : BuiltConventionBase<IPropertyInspector, IPropertyInstance>, IPropertyConvention
    {
        public BuiltPropertyConvention(Action<IAcceptanceCriteria<IPropertyInspector>> accept, Action<IPropertyInstance> convention)
            : base(accept, convention)
        { }
    }
}