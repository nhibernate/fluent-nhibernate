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
        private readonly ConventionOverrides conventionOverrides;

        public AutoMapColumn(ConventionOverrides conventionOverrides)
        {
            this.conventionOverrides = conventionOverrides;
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
            var conventions = conventionOverrides.Finder
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