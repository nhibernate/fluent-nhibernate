using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class AutoMapping<T> : ClassMap<T>, IAutoClasslike, IPropertyIgnorer
    {
        private readonly IList<string> mappedProperties;

        public AutoMapping(IList<string> mappedProperties)
        {
            this.mappedProperties = mappedProperties;
        }

        public IEnumerable<string> PropertiesMapped
        {
            get { return mappedProperties; }
        }

        void IAutoClasslike.DiscriminateSubClassesOnColumn(string column)
        {
            DiscriminateSubClassesOnColumn(column);
        }

        IEnumerable<string> IMappingProvider.GetIgnoredProperties()
        {
            return mappedProperties;
        }

        void IAutoClasslike.AlterModel(ClassMappingBase classMapping)
        {
            classMapping.MergeAttributes(attributes.CloneInner());

            if (classMapping is ClassMapping)
            {
                if (id != null)
                    ((ClassMapping)classMapping).Id = id.GetIdentityMapping();

                if (version != null)
                    ((ClassMapping)classMapping).Version = version.GetVersionMapping();
            }

            foreach (var property in Properties)
                classMapping.AddOrReplaceProperty(property.GetPropertyMapping());

            foreach (var collection in collections)
                classMapping.AddOrReplaceCollection(collection.GetCollectionMapping());

            foreach (var component in Components)
                classMapping.AddOrReplaceComponent(component.GetComponentMapping());

            foreach (var oneToOne in oneToOnes)
                classMapping.AddOrReplaceOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var reference in references)
                classMapping.AddOrReplaceReference(reference.GetManyToOneMapping());

            foreach (var any in anys)
                classMapping.AddOrReplaceAny(any.GetAnyMapping());

            // TODO: Add other mappings
        }

        protected override OneToManyPart<TChild> HasMany<TChild>(PropertyInfo property)
        {
            mappedProperties.Add(property.Name);
            return base.HasMany<TChild>(property);
        }

        public void IgnoreProperty(Expression<Func<T, object>> expression)
        {
            mappedProperties.Add(ReflectionHelper.GetProperty(expression).Name);
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperty(string name)
        {
            mappedProperties.Add(name);

            return this;
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperties(string first, string second, params string[] others)
        {
            (others ?? new string[0])
                .Concat(new[] { first, second })
                .Each(mappedProperties.Add);

            return this;
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperties(Func<PropertyInfo, bool> predicate)
        {
            typeof(T).GetProperties()
                .Where(predicate)
                .Each(x => mappedProperties.Add(x.Name));

            return this;
        }

        public override IdentityPart Id(Expression<Func<T, object>> expression)
        {
            mappedProperties.Add(ReflectionHelper.GetProperty(expression).Name);
            return base.Id(expression);
        }

        protected override PropertyPart Map(PropertyInfo property, string columnName)
        {
            mappedProperties.Add(property.Name);
            return base.Map(property, columnName);
        }

        protected override ManyToOnePart<TOther> References<TOther>(PropertyInfo property, string columnName)
        {
            mappedProperties.Add(property.Name);
            return base.References<TOther>(property, columnName);
        }

        protected override ManyToManyPart<TChild> HasManyToMany<TChild>(PropertyInfo property)
        {
            mappedProperties.Add(property.Name);
            return base.HasManyToMany<TChild>(property);
        }

        protected override ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            mappedProperties.Add(property.Name);

            if (action == null)
                action = c => { };

            return base.Component(property, action);
        }

        public override IdentityPart Id(Expression<Func<T, object>> expression, string column)
        {
            mappedProperties.Add(ReflectionHelper.GetProperty(expression).Name);
            return base.Id(expression, column);
        }

        protected override OneToOnePart<TOther> HasOne<TOther>(PropertyInfo property)
        {
            mappedProperties.Add(property.Name);
            return base.HasOne<TOther>(property);
        }

        protected override VersionPart Version(PropertyInfo property)
        {
            mappedProperties.Add(property.Name);
            return base.Version(property);
        }

		public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn, Action<AutoJoinedSubClassPart<TSubclass>> action)
			where TSubclass : T
        {
            var genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var joinedclass = (AutoJoinedSubClassPart<TSubclass>)Activator.CreateInstance(genericType, keyColumn);

            action(joinedclass);

            subclasses[typeof(TSubclass)] = joinedclass;

		    return joinedclass;
        }

        public IAutoClasslike JoinedSubClass(Type type, string keyColumn)
        {
            var genericType = typeof (AutoJoinedSubClassPart<>).MakeGenericType(type);
            var joinedclass = (ISubclassMappingProvider)Activator.CreateInstance(genericType, keyColumn);

            // remove any mappings for the same type, then re-add
            subclasses[type] = joinedclass;

            return (IAutoClasslike)joinedclass;
        }

        public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn)
			where TSubclass : T
		{
			return JoinedSubClass<TSubclass>(keyColumn, null);
		}

		public AutoSubClassPart<TSubclass> SubClass<TSubclass>(object discriminatorValue, Action<AutoSubClassPart<TSubclass>> action)
			where TSubclass : T
        {
            var genericType = typeof(AutoSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var subclass = (AutoSubClassPart<TSubclass>)Activator.CreateInstance(genericType, null, discriminatorValue);
            
            action(subclass);

            // remove any mappings for the same type, then re-add
            subclasses[typeof(TSubclass)] = subclass;

		    return subclass;
        }

        public AutoSubClassPart<TSubclass> SubClass<TSubclass>(object discriminatorValue)
			where TSubclass : T
		{
			return SubClass<TSubclass>(discriminatorValue, null);
		}

        public IAutoClasslike SubClass(Type type, string discriminatorValue)
        {
            var genericType = typeof(AutoSubClassPart<>).MakeGenericType(type);
            var subclass = (ISubclassMappingProvider)Activator.CreateInstance(genericType, null, discriminatorValue);

            // remove any mappings for the same type, then re-add
            subclasses[type] = subclass;

            return (IAutoClasslike)subclass;
        }
    }
}