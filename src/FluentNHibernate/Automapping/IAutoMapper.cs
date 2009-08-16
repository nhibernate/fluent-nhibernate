using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public interface IAutoMapper
    {
        bool MapsProperty(PropertyInfo property);
        void Map(ClassMappingBase classMap, PropertyInfo property);
    }
}