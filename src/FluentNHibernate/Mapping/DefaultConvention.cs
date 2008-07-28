using System;

namespace FluentNHibernate.Mapping
{
    public class DefaultConvention : ITypeConvention
    {
        public bool CanHandle(Type type)
        {
            return true;
        }

        public void AlterMap(IProperty property)
        {
            property.SetAttributeOnPropertyElement("type", TypeMapping.GetTypeString(property.PropertyType));
        }
    }
}