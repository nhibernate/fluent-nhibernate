using System;

namespace FluentNHibernate.Mapping
{
    public interface ITypeConvention
    {
        bool CanHandle(Type type);
        void AlterMap(IProperty propertyMapping);
        
    }
}