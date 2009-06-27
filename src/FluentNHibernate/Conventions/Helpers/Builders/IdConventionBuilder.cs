using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class IdConventionBuilder : IConventionBuilder<IIdConvention, IIdentityInspector, IIdentityAlteration, IIdentityInstance>
    {
        public IIdConvention Always(Action<IIdentityInstance> convention)
        {
            return new BuiltIdConvention(accept => { }, convention);
        }

        public IIdConvention When(Action<IAcceptanceCriteria<IIdentityInspector>> expectations, Action<IIdentityInstance> convention)
        {
            return new BuiltIdConvention(expectations, convention);
        }
    }
}