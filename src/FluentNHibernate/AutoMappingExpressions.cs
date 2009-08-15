using System;
using System.Reflection;
using FluentNHibernate.Automapping;

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
		public Func<Type, bool> IsDiscriminated;
        public Func<Type, string> DiscriminatorColumn = t => "discriminator";
        public Func<Type, SubclassStrategy> SubclassStrategy = t => Automapping.SubclassStrategy.JoinedSubclass;

		public AutoMappingExpressions()
		{
			IsDiscriminated = t => SubclassStrategy(t) == Automapping.SubclassStrategy.Subclass;
		}
	}
}