using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltPropertyConvention : BuiltConventionBase<IPropertyInspector, IPropertyAlteration>, IPropertyConvention
    {
        public BuiltPropertyConvention(Action<IAcceptanceCriteria<IPropertyInspector>> accept, Action<IPropertyAlteration, IPropertyInspector> convention)
            : base(accept, convention)
        { }
    }
}