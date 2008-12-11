using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapIdentity : IAutoMapper
    {
        private readonly Conventions conventions;

        public AutoMapIdentity(Conventions conventions)
        {
            this.conventions = conventions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            if (property.ReflectedType.BaseType != typeof(object))
                return false;

            return conventions.FindIdentity.Invoke(property);
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            classMap.Id(ExpressionBuilder.Create<T>(property));
        }
    }
}