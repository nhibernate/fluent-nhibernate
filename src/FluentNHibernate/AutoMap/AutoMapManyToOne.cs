using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapManyToOne : IAutoMapper
    {
        public bool MapsProperty(PropertyInfo property)
        {
            return ((property.PropertyType.Namespace != "System")
                    && (property.PropertyType.Namespace != "System.Collections.Generic"));
        }

        public void Map<T>(ClassMap<T> classMap, PropertyInfo property)
        {
            classMap.References(ExpressionBuilder.Create<T>(property));
        }
    }
}