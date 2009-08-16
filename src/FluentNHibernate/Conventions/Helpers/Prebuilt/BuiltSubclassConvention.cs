using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltSubclassConvention : BuiltConventionBase<ISubclassInspector, ISubclassInstance>, ISubclassConvention
    {
        public BuiltSubclassConvention(Action<IAcceptanceCriteria<ISubclassInspector>> accept, Action<ISubclassInstance> convention) : base(accept, convention)
        {}
    }
}