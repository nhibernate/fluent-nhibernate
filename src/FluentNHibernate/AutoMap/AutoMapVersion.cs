using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapVersion : IAutoMapper
    {
        public bool MapsProperty(PropertyInfo property)
        {
            return (property.Name.ToLower() == "version") || (property.Name.ToLower() == "timestamp");
        }

        public void Map<T>(ClassMap<T> classMap, PropertyInfo property)
        {
            classMap.Version(ExpressionBuilder.Create<T>(property));
        }
    }
}