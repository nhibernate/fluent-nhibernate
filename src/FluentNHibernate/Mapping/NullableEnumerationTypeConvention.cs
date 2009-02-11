using System;

namespace FluentNHibernate.Mapping
{
    public class NullableEnumerationTypeConvention : ITypeConvention
    {
        public bool CanHandle(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && type.GetGenericArguments()[0].IsEnum;
        }

        public void AlterMap(IProperty propertyMapping)
        {
            var enumerationType = propertyMapping.PropertyType.GetGenericArguments()[0];
            var mapperType = typeof(GenericEnumMapper<>).MakeGenericType(enumerationType);

            propertyMapping.CustomTypeIs(mapperType);
            propertyMapping.Nullable();
            propertyMapping.CustomSqlTypeIs("string");
        }
    }
}