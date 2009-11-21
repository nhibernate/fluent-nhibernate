using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public interface IAutoMapper
    {
        bool MapsProperty(Member property);
        void Map(ClassMappingBase classMap, Member property);
    }
}