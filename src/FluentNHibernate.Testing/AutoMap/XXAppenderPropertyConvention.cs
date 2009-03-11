using System.Runtime.Remoting.Messaging;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Testing.AutoMap
{
    public class XXAppenderPropertyConvention : IPropertyConvention
    {
        public bool Accept(IProperty property)
        {
            return true;
        }

        public void Apply(IProperty propertyMapping)
        {
            propertyMapping.ColumnName(propertyMapping.Property.Name + "XX");
        }
    }
}
