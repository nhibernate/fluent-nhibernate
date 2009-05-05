using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
    internal class BuiltPropertyConvention : BuiltConventionBase<IProperty>, IPropertyConvention
    {
        public BuiltPropertyConvention(Func<IProperty, bool> accept, Action<IProperty> convention)
            : base(accept, convention)
        { }
    }
}