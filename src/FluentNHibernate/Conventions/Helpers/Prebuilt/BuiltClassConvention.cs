using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltClassConvention : BuiltConventionBase<IClassMap>, IClassConvention
    {
        public BuiltClassConvention(Func<IClassMap, bool> accept, Action<IClassMap> convention)
            : base(accept, convention)
        {}
    }
}