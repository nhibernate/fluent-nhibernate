using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapIdentity : IAutoMapper
    {
        public bool MapsProperty(PropertyInfo property)
        {
            return property.Name == "Id";
        }

        public void Map<T>(ClassMap<T> classMap, PropertyInfo property)
        {
            classMap.Id(ExpressionBuilder.Create<T>(property));
        }
    }
}