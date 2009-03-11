using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Specifies a custom type (of <see cref="GenericEnumMapper{TEnum}"/>) for any properties
    /// that are an enum.
    /// </summary>
    public class EnumerationPropertyConvention : IPropertyConvention
    {
        public bool Accept(IProperty target)
        {
            return target.PropertyType.IsEnum && !target.HasAttribute("type");
        }

        public void Apply(IProperty target)
        {
            var mapperType = typeof(GenericEnumMapper<>).MakeGenericType(target.PropertyType);
            
            target.CustomTypeIs(mapperType);
        }
    }
}