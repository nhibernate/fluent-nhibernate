using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.AutoMap
{
    public interface IAutoMapper
    {
        bool MapsProperty(PropertyInfo property);
        void Map<T>(AutoMap<T> classMap, PropertyInfo property);
        void Map(ClassMapping classMap, PropertyInfo property);
    }
}