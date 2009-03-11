using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class EnumerationPropertyConvention : IPropertyConvention
    {
        public bool Accept(IProperty target)
        {
            return target.PropertyType.IsEnum && !target.HasAttribute("type");
        }

        public void Apply(IProperty target, ConventionOverrides overrides)
        {
            Type mapperType = typeof(GenericEnumMapper<>).MakeGenericType(target.PropertyType);
            
            target.CustomTypeIs(mapperType);
            target.CustomSqlTypeIs("string");
        }
    }
}