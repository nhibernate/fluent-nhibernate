using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    public class CompositeIdConventionBuilder : IConventionBuilder<ICompositeIdentityConvention, ICompositeIdentityInspector, ICompositeIdentityInstance>
    {
        public ICompositeIdentityConvention Always(Action<ICompositeIdentityInstance> convention)
        {
            return new BuiltCompositeIdConvention(accept => { }, convention);
        }

        public ICompositeIdentityConvention When(Action<IAcceptanceCriteria<ICompositeIdentityInspector>> expectations, Action<ICompositeIdentityInstance> convention)
        {
            return new BuiltCompositeIdConvention(expectations, convention);
        }
    }
}