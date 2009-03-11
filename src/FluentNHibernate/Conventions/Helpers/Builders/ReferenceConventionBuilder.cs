using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class ReferenceConventionBuilder : IConventionBuilder<IReferenceConvention, IManyToOnePart>
    {
        public IReferenceConvention Always(Action<IManyToOnePart> convention)
        {
            return new BuiltReferenceConvention(x => true, convention);
        }

        public IReferenceConvention When(Func<IManyToOnePart, bool> isTrue, Action<IManyToOnePart> convention)
        {
            return new BuiltReferenceConvention(isTrue, convention);
        }
    }
}