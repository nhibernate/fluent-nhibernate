using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class ReferenceConventionBuilder : IConventionBuilder<IReferenceConvention, IManyToOneInspector, IManyToOneAlteration>
    {
        public IReferenceConvention Always(Action<IManyToOneAlteration> convention)
        {
            return new BuiltReferenceConvention(accept => { }, (a, i) => convention(a));
        }

        public IReferenceConvention Always(Action<IManyToOneAlteration, IManyToOneInspector> convention)
        {
            return new BuiltReferenceConvention(accept => { }, convention);
        }

        public IReferenceConvention When(Action<IAcceptanceCriteria<IManyToOneInspector>> expectations, Action<IManyToOneAlteration> convention)
        {
            return new BuiltReferenceConvention(expectations, (a, i) => convention(a));
        }

        public IReferenceConvention When(Action<IAcceptanceCriteria<IManyToOneInspector>> expectations, Action<IManyToOneAlteration, IManyToOneInspector> convention)
        {
            return new BuiltReferenceConvention(expectations, convention);
        }
    }
}