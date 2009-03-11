using System;
using System.Reflection;
using FluentNHibernate.Conventions;

namespace FluentNHibernate
{
    public class AutoMapConventionOverrides : ConventionOverrides
    {
        public Func<PropertyInfo, bool> FindIdentity = p => p.Name == "Id";
        public Func<Type, Type, Type> GetParentSideForManyToMany = (one, two) => one.FullName.CompareTo(two.FullName) < 0 ? one : two;
        public Func<PropertyInfo, bool> FindMappablePrivateProperties;
        public Func<Type, bool> IsBaseType = b => b == typeof(object);
        public Func<Type, bool> IsComponentType = type => false;
        public Func<PropertyInfo, string> GetComponentColumnPrefix = property => property.Name;

        public AutoMapConventionOverrides()
        {}

        public AutoMapConventionOverrides(IConventionFinder finder)
            : base(finder)
        {}
    }
}