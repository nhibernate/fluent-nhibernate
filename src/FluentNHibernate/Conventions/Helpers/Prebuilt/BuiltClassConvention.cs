using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltClassConvention : BuiltConventionBase<IClassInspector, IClassInstance>, IClassConvention
    {
        public BuiltClassConvention(Action<IAcceptanceCriteria<IClassInspector>> accept, Action<IClassInstance> convention)
            : base(accept, convention)
        {}
    }
}