using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Sets the type attribute on property mappings
    /// </summary>
    public class PropertyTypeConvention : IPropertyConvention
    {
        public bool Accept(IProperty target)
        {
            // TODO: Fix this with convention DSL, tests will fail until then
            //return !target.HasAttribute("type") && !target.PropertyType.IsEnum;
            throw new NotSupportedException("Awaiting convention DSL");
        }

        public void Apply(IProperty target)
        {
            target.CustomTypeIs(target.PropertyType);
        }
    }
}