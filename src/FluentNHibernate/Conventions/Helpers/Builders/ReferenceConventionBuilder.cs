using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class ReferenceConventionBuilder : IConventionBuilder<IReferenceConvention, IManyToOneInspector, IManyToOneAlteration, IManyToOneInstance>
    {
        public IReferenceConvention Always(Action<IManyToOneInstance> convention)
        {
            return new BuiltReferenceConvention(accept => { }, convention);
        }

        public IReferenceConvention When(Action<IAcceptanceCriteria<IManyToOneInspector>> expectations, Action<IManyToOneInstance> convention)
        {
            return new BuiltReferenceConvention(expectations, convention);
        }
    }
}