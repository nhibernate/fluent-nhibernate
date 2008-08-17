using System;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public interface IAutoMapper
    {
        bool MapsProperty(PropertyInfo property);
        void Map<T>(AutoMap<T> classMap, PropertyInfo property);
    }
}