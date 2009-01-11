using System;

namespace FluentNHibernate.Mapping
{
    public class DefaultConvention : ITypeConvention
    {
        public bool CanHandle(Type type)
        {
            return true;
        }

        public void AlterMap(IProperty propertyMapping)
        {            
            if(!propertyMapping.HasAttribute("type"))
                propertyMapping.SetAttribute("type", TypeMapping.GetTypeString(propertyMapping.PropertyType));
        }
    }
}