using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltVersionConvention : BuiltConventionBase<IVersionInspector, IVersionInstance>, IVersionConvention
    {
        public BuiltVersionConvention(Action<IAcceptanceCriteria<IVersionInspector>> accept, Action<IVersionInstance> convention)
            : base(accept, convention)
        { }
    }
}