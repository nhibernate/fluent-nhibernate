using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.AutoMap
{
    public interface IAutoMapper
    {
        bool MapsProperty(PropertyInfo property);
        void Map(ClassMapping classMap, PropertyInfo property);
        void Map(SubclassMapping classMap, PropertyInfo property);
        void Map(JoinedSubclassMapping classMap, PropertyInfo property);
    }
}