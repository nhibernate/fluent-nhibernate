using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltIdConvention : BuiltConventionBase<IIdentityPart>, IIdConvention
    {
        public BuiltIdConvention(Func<IIdentityPart, bool> accept, Action<IIdentityPart> convention)
            : base(accept, convention)
        {}
    }
}