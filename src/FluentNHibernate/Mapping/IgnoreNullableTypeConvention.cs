using System;

namespace FluentNHibernate.Mapping
{
    public class IgnoreNullableTypeConvention : ITypeConvention
    {
        public bool CanHandle(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && !type.GetGenericArguments()[0].IsEnum;
        }

        public void AlterMap(IProperty propertyMapping)
        {
            // no-op;
        }
    }
}