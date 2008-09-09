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
            if(!property.HasAttribute("type"))
                property.SetAttribute("type", TypeMapping.GetTypeString(property.PropertyType));
        }
    }
}