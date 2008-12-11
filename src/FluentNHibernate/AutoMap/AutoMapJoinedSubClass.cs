using System.Reflection;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapJoinedSubClass : IAutoMapper
    {
        public bool MapsProperty(PropertyInfo property)
        {
            throw new System.NotImplementedException();
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            throw new System.NotImplementedException();
        }
    }
}