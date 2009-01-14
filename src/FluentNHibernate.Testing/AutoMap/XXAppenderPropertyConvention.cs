using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.AutoMap
{
    internal class XXAppenderPropertyConvention : IPropertyConvention
    {
        public bool CanHandle(IProperty property)
        {
            return true;
        }

        public void Process(IProperty propertyMapping)
        {
            propertyMapping.TheColumnNameIs(propertyMapping.Property.Name + "XX");
        }
    }
}