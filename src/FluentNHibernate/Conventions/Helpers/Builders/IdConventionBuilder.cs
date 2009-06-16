using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class IdConventionBuilder : IConventionBuilder<IIdConvention, IIdentityInspector, IIdentityAlteration>
    {
        public IIdConvention Always(Action<IIdentityAlteration> convention)
        {
            return new BuiltIdConvention(accept => { }, (a, i) => convention(a));
        }

        public IIdConvention Always(Action<IIdentityAlteration, IIdentityInspector> convention)
        {
            return new BuiltIdConvention(accept => { }, convention);
        }

        public IIdConvention When(Action<IAcceptanceCriteria<IIdentityInspector>> expectations, Action<IIdentityAlteration> convention)
        {
            return new BuiltIdConvention(expectations, (a, i) => convention(a));
        }

        public IIdConvention When(Action<IAcceptanceCriteria<IIdentityInspector>> expectations, Action<IIdentityAlteration, IIdentityInspector> convention)
        {
            return new BuiltIdConvention(expectations, convention);
        }
    }
}