using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Conventions;

namespace FluentNHibernate.Testing.AutoMap
{
    public class XXAppenderPropertyConvention : IPropertyConvention
    {
        public bool Accept(IProperty property)
        {
            return true;
        }

        public void Apply(IProperty propertyMapping, ConventionOverrides overrides)
        {
            propertyMapping.ColumnName(propertyMapping.Property.Name + "XX");
        }
    }
}