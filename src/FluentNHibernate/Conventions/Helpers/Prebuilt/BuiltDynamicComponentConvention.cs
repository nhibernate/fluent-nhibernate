using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltDynamicComponentConvention : BuiltConventionBase<IDynamicComponent>, IDynamicComponentConvention
    {
        public BuiltDynamicComponentConvention(Func<IDynamicComponent, bool> accept, Action<IDynamicComponent> convention)
            : base(accept, convention)
        { }
    }
}