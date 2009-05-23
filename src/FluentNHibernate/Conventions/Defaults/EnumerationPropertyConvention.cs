using System;
using FluentNHibernate.Mapping;

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
            // TODO: Fix this with convention DSL, tests will fail until then
            //return target.PropertyType.IsEnum && !target.HasAttribute("type");
            throw new NotSupportedException("Awaiting convention DSL");
        }

        public void Apply(IProperty target)
        {
            var mapperType = typeof(GenericEnumMapper<>).MakeGenericType(target.PropertyType);
            
            target.CustomTypeIs(mapperType);
        }
    }
}