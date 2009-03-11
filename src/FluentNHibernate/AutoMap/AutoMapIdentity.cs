using System;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapIdentity : IAutoMapper
    {
        private readonly AutoMapConventionOverrides conventions;

        public AutoMapIdentity(AutoMapConventionOverrides conventions)
        {
            this.conventions = conventions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            return conventions.FindIdentity.Invoke(property);
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (classMap is AutoJoinedSubClassPart<T>)
                return;

            classMap.Id(ExpressionBuilder.Create<T>(property));
        }
    }
}