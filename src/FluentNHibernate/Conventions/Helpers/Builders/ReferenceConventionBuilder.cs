using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class ReferenceConventionBuilder : IConventionBuilder<IReferenceConvention, IManyToOneInspector, IManyToOneInstance>
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