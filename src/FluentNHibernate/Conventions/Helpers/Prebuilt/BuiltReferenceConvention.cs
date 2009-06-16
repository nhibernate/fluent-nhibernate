using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltReferenceConvention : BuiltConventionBase<IManyToOneInspector, IManyToOneAlteration>, IReferenceConvention
    {
        public BuiltReferenceConvention(Action<IAcceptanceCriteria<IManyToOneInspector>> accept, Action<IManyToOneAlteration, IManyToOneInspector> convention)
            : base(accept, convention)
        { }
    }
}