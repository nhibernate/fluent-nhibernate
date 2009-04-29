using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Sets the type attribute on property mappings
    /// </summary>
    public class PropertyTypeConvention : IPropertyConvention
    {
        public bool Accept(IProperty target)
        {
            return !target.HasAttribute("type") && !target.PropertyType.IsEnum;
        }

        public void Apply(IProperty target)
        {
            target.CustomTypeIs(target.PropertyType);
        }
    }
}