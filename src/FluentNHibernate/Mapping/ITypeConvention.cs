using System;

namespace ShadeTree.DomainModel.Mapping
{
    public interface ITypeConvention
    {
        bool CanHandle(Type type);
        void AlterMap(IProperty property);
        
    }
}