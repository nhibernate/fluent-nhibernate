using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.AutoMap
{
    public class AutoMap<T> : ClassMap<T>, IPropertyIgnorer
    {
        private IList<string> propertiesMapped = new List<string>();
        private readonly Dictionary<Type, IJoinedSubclass> joinedSubClasses = new Dictionary<Type, IJoinedSubclass>();
        private readonly Dictionary<Type, ISubclass> automappedSubclasses = new Dictionary<Type, ISubclass>();

        public IList<string> PropertiesMapped
        {
            get { return propertiesMapped; }
            set { propertiesMapped = value; }
        }

        protected override OneToManyPart<TChild> HasMany<TChild>(PropertyInfo property)
        {
            propertiesMapped.Add(property.Name);
            return base.HasMany<TChild>(property);
        }

        public void IgnoreProperty(Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression).Name);
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperty(string name)
        {
            propertiesMapped.Add(name);

            return this;
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperties(string first, string second, params string[] others)
        {
            (others ?? new string[0])
                .Concat(new[] { first, second })
                .Each(propertiesMapped.Add);

            return this;
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperties(Func<PropertyInfo, bool> predicate)
        {
            typeof(T).GetProperties()
                .Where(predicate)
                .Each(x => propertiesMapped.Add(x.Name));

            return this;
        }

        public override IIdentityPart Id(Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression).Name);
            return base.Id(expression);
        }

        protected override PropertyMap Map(PropertyInfo property, string columnName)
        {
            propertiesMapped.Add(property.Name);
            return base.Map(property, columnName);
        }

        protected override ManyToOnePart<TOther> References<TOther>(PropertyInfo property, string columnName)
        {
            propertiesMapped.Add(property.Name);
            return base.References<TOther>(property, columnName);
        }

        protected override ManyToManyPart<TChild> HasManyToMany<TChild>(PropertyInfo property)
        {
            propertiesMapped.Add(property.Name);
            return base.HasManyToMany<TChild>(property);
        }

        protected override ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            propertiesMapped.Add(property.Name);

            if (action == null)
                action = c => { };

            return base.Component(property, action);
        }

        public override IIdentityPart Id(Expression<Func<T, object>> expression, string column)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression).Name);
            return base.Id(expression, column);
        }

        protected override OneToOnePart<TOther> HasOne<TOther>(PropertyInfo property)
        {
            propertiesMapped.Add(property.Name);
            return base.HasOne<TOther>(property);
        }

        protected override VersionPart Version(PropertyInfo property)
        {
            propertiesMapped.Add(property.Name);
            return base.Version(property);
        }

		public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn, Action<AutoJoinedSubClassPart<TSubclass>> action)
			where TSubclass : T
        {
			IJoinedSubclass joinedSubclass;
			if (joinedSubClasses.TryGetValue(typeof(TSubclass), out joinedSubclass))
				return (AutoJoinedSubClassPart<TSubclass>)joinedSubclass;

			var autoJoinedclass = new AutoJoinedSubClassPart<TSubclass>(keyColumn);
            if (action != null) action(autoJoinedclass);
            AddPart(autoJoinedclass);
            joinedSubClasses[typeof(TSubclass)] = autoJoinedclass;
			return autoJoinedclass;
        }

		public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn)
			where TSubclass : T
		{
			return JoinedSubClass<TSubclass>(keyColumn, null);
		}

        public IJoinedSubclass JoinedSubClass(Type type, string keyColumn)
        {
            if (joinedSubClasses.ContainsKey(type))
                return joinedSubClasses[type];

            var genericType = typeof (AutoJoinedSubClassPart<>).MakeGenericType(type);
            var joinedclass = (IJoinedSubclass)Activator.CreateInstance(genericType, keyColumn);                      
            AddPart(joinedclass);
            joinedSubClasses[type] = joinedclass;
            return joinedclass;
        }

		public AutoSubClassPart<TSubclass> SubClass<TSubclass>(object discriminatorValue, Action<AutoSubClassPart<TSubclass>> action)
			where TSubclass : T
        {
			ISubclass subclass;
			if (automappedSubclasses.TryGetValue(typeof(TSubclass), out subclass))
				return (AutoSubClassPart<TSubclass>)subclass;

			var autoSubclass = new AutoSubClassPart<TSubclass>(discriminatorValue);
			if (action != null) action(autoSubclass);
            AddPart(autoSubclass);
            automappedSubclasses[typeof(TSubclass)] = autoSubclass;
			return autoSubclass;
        }

		public AutoSubClassPart<TSubclass> SubClass<TSubclass>(object discriminatorValue)
			where TSubclass : T
		{
			return SubClass<TSubclass>(discriminatorValue, null);
		}

        public ISubclass SubClass(Type type, object discriminatorValue)
        {
            if (automappedSubclasses.ContainsKey(type))
                return automappedSubclasses[type];

            var genericType = typeof(AutoSubClassPart<>).MakeGenericType(type);
            var subclass = (ISubclass)Activator.CreateInstance(genericType, discriminatorValue);

            AddPart(subclass);
            automappedSubclasses[type] = subclass;

            return subclass;
        }

        public bool CanMapProperty(PropertyInfo property)
        {
            if (this is AutoJoinedSubClassPart<T> || this is AutoSubClassPart<T>)
            {
                if (property.DeclaringType != typeof(T))
                    return false;
            }

            return true;
        }

		protected override IEnumerable<ISubclass> Subclasses
		{
			get { return automappedSubclasses.Values; }
		}

		protected override IEnumerable<IJoinedSubclass> JoinedSubclasses
		{
			get { return joinedSubClasses.Values; }
		}
    }
}