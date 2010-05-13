using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltCompositeIdConvention : BuiltConventionBase<ICompositeIdentityInspector, ICompositeIdentityInstance>, ICompositeIdentityConvention, ICompositeIdentityConventionAcceptance
    {
        public BuiltCompositeIdConvention(Action<IAcceptanceCriteria<ICompositeIdentityInspector>> accept, Action<ICompositeIdentityInstance> convention)
            : base(accept, convention)
        {}
    }
}