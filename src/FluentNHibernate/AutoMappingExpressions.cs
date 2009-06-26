using System;
using System.Reflection;
using FluentNHibernate.AutoMap;

namespace FluentNHibernate
{
    public class AutoMappingExpressions
    {
        public Func<PropertyInfo, bool> FindIdentity = p => p.Name == "Id";
        public Func<Type, Type, Type> GetParentSideForManyToMany = (one, two) => one.FullName.CompareTo(two.FullName) < 0 ? one : two;
        public Func<PropertyInfo, bool> FindMappablePrivateProperties;
        public Func<Type, bool> IsBaseType = b => b == typeof(object);
        public Func<Type, bool> IsConcreteBaseType = b => false;
        public Func<Type, bool> IsComponentType = type => false;
        public Func<PropertyInfo, string> GetComponentColumnPrefix = property => property.Name;
        public Func<Type, string> DiscriminatorColumn = t => "discriminator";
        public SubclassStrategy SubclassStrategy { get; set; }

        public AutoMappingExpressions()
        {
            SubclassStrategy = SubclassStrategy.JoinedSubclass;
        }
    }
}