using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltIdConvention : BuiltConventionBase<IIdentityInspector, IIdentityInstance>, IIdConvention
    {
        public BuiltIdConvention(Action<IAcceptanceCriteria<IIdentityInspector>> accept, Action<IIdentityInstance> convention)
            : base(accept, convention)
        {}
    }
}