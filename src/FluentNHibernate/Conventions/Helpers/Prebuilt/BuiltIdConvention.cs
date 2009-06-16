using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltIdConvention : BuiltConventionBase<IIdentityInspector, IIdentityAlteration>, IIdConvention
    {
        public BuiltIdConvention(Action<IAcceptanceCriteria<IIdentityInspector>> accept, Action<IIdentityAlteration, IIdentityInspector> convention)
            : base(accept, convention)
        {}
    }
}