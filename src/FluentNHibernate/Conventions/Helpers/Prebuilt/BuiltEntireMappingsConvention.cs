using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltEntireMappingsConvention : BuiltConventionBase<IEnumerable<IClassMap>>, IEntireMappingsConvention
    {
        public BuiltEntireMappingsConvention(Func<IEnumerable<IClassMap>, bool> accept, Action<IEnumerable<IClassMap>> convention)
            : base(accept, convention)
        { }
    }
}