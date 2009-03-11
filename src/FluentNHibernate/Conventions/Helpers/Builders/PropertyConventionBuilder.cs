using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers.Builders
{
    internal class PropertyConventionBuilder : IConventionBuilder<IPropertyConvention, IProperty>
    {
        public IPropertyConvention Always(Action<IProperty> convention)
        {
            return new BuiltPropertyConvention(x => true, convention);
        }

        public IPropertyConvention When(Func<IProperty, bool> isTrue, Action<IProperty> convention)
        {
            return new BuiltPropertyConvention(isTrue, convention);
        }
    }
}