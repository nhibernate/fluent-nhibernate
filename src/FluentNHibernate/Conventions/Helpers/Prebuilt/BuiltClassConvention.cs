using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltClassConvention : BuiltConventionBase<IClassInspector, IClassAlteration>, IClassConvention
    {
        public BuiltClassConvention(Action<IAcceptanceCriteria<IClassInspector>> accept, Action<IClassAlteration, IClassInspector> convention)
            : base(accept, convention)
        {}
    }
}