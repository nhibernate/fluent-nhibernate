using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapColumn : IAutoMapper
    {
        private IConventionFinder conventionFinder;

        public AutoMapColumn(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            if (HasExplicitTypeConvention(property))
                return true;

            if (property.CanWrite)
                return property.PropertyType.Namespace == "System";

            return false;
        }

        private bool HasExplicitTypeConvention(PropertyInfo property)
        {
            var conventions = conventionFinder
                .Find<IUserTypeConvention>()
                .Where(c => c.Accept(property.PropertyType));

            return conventions.FirstOrDefault() != null;
        }

        public void Map<T>(AutoMap<T> classMap, PropertyInfo property)
        {
            if (!classMap.CanMapProperty(property))
                return;

            classMap.Map(ExpressionBuilder.Create<T>(property));
        }
    }
}