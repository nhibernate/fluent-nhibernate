using System.Runtime.Remoting.Messaging;
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
            if (CallContext.GetData("XXAppender") != null)
                propertyMapping.TheColumnNameIs(propertyMapping.Property.Name + "XX");
        }
    }
}
