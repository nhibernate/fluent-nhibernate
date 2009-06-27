using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltReferenceConvention : BuiltConventionBase<IManyToOneInspector, IManyToOneInstance>, IReferenceConvention
    {
        public BuiltReferenceConvention(Action<IAcceptanceCriteria<IManyToOneInspector>> accept, Action<IManyToOneInstance> convention)
            : base(accept, convention)
        { }
    }
}