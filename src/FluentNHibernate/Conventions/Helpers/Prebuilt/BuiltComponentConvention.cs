using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltComponentConvention : BuiltConventionBase<IComponent>, IComponentConvention
    {
        public BuiltComponentConvention(Func<IComponent, bool> accept, Action<IComponent> convention)
            : base(accept, convention)
        { }
    }
}