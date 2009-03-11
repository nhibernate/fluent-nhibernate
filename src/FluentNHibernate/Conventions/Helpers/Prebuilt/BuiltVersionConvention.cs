using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltVersionConvention : BuiltConventionBase<IVersion>, IVersionConvention
    {
        public BuiltVersionConvention(Func<IVersion, bool> accept, Action<IVersion> convention)
            : base(accept, convention)
        { }
    }
}