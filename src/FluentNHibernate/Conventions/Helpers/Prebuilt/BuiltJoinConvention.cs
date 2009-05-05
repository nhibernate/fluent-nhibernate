using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltJoinConvention : BuiltConventionBase<IJoin>, IJoinConvention
    {
        public BuiltJoinConvention(Func<IJoin, bool> accept, Action<IJoin> convention)
            : base(accept, convention)
        { }
    }
}