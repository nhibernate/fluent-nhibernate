using System;

namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class EnumerationPropertyConvention : IPropertyConvention
    {
        public bool Accept(IProperty target)
        {
            return target.PropertyType.IsEnum;
        }

        public void Apply(IProperty target, ConventionOverrides overrides)
        {
            Type mapperType = typeof(GenericEnumMapper<>).MakeGenericType(target.PropertyType);
            
            target.CustomTypeIs(mapperType);
            target.CustomSqlTypeIs("string");
        }
    }
}