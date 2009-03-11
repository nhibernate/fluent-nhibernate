using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class PropertyTypeConvention : IPropertyConvention
    {
        public bool Accept(IProperty target)
        {
            return !target.HasAttribute("type");
        }

        public void Apply(IProperty target, ConventionOverrides overrides)
        {            
            target.SetAttribute("type", TypeMapping.GetTypeString(target.PropertyType));
        }
    }
}