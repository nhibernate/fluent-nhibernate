using System;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;

namespace FluentNHibernate
{
    public class AutoMappingExpressions
    {
        /// <summary>
        /// Determines whether a property is the identity of an entity.
        /// </summary>
        public Func<Member, bool> FindIdentity = p => p.Name == "Id";

        public Func<Type, Type, Type> GetParentSideForManyToMany = (one, two) => one.FullName.CompareTo(two.FullName) < 0 ? one : two;
        public Func<Member, bool> FindMappablePrivateProperties;

        [Obsolete("Use IgnoreBase<T> or IgnoreBase(Type): AutoMap.AssemblyOf<Entity>().IgnoreBase(typeof(Parent<>))")]
        public Func<Type, bool> IsBaseType = b => b == typeof(object);

        public Func<Type, bool> IsConcreteBaseType = b => false;
        public Func<Type, bool> IsComponentType = type => false;
        public Func<Member, string> GetComponentColumnPrefix = property => property.Name;
		public Func<Type, bool> IsDiscriminated;
        public Func<Type, string> DiscriminatorColumn = t => "discriminator";
        public Func<Type, SubclassStrategy> SubclassStrategy = t => Automapping.SubclassStrategy.JoinedSubclass;

        /// <summary>
        /// Determines whether an abstract class is a layer supertype or part of a mapped inheritance hierarchy.
        /// </summary>
        public Func<Type, bool> AbstractClassIsLayerSupertype = t => true;

        /// <summary>
        /// Specifies the value column used in a table of simple types. 
        /// </summary>
        public Func<Member, string> SimpleTypeCollectionValueColumn = t => "Value";

        public AutoMappingExpressions()
		{
			IsDiscriminated = t => SubclassStrategy(t) == Automapping.SubclassStrategy.Subclass;
		}
	}
}