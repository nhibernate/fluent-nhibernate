using System;

namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class IgnoreNullablePropertyConvention : IPropertyConvention
    {
        public bool Accept(IProperty target)
        {
            var type = target.PropertyType;

            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && !type.GetGenericArguments()[0].IsEnum;
        }

        public void Apply(IProperty target, ConventionOverrides overrides)
        {
            // no-op;
        }
    }
}