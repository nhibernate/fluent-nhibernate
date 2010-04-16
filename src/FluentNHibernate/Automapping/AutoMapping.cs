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
        readonly IList<Member> mappedMembers;

        public AutoMapping(IList<Member> mappedMembers)
        {
            this.mappedMembers = mappedMembers;
        }

        void IAutoClasslike.DiscriminateSubClassesOnColumn(string column)
        {
            DiscriminateSubClassesOnColumn(column);
        }

        IEnumerable<Member> IMappingProvider.GetIgnoredProperties()
        {
            return mappedMembers;
        }

        void IAutoClasslike.AlterModel(ClassMappingBase mapping)
        {
            mapping.MergeAttributes(attributes.CloneInner());

            if (mapping is ClassMapping)
            {
                var classMapping = (ClassMapping)mapping;

                if (id != null)
                    classMapping.Id = id.GetIdentityMapping();

                if (compositeId != null)
                    classMapping.Id = compositeId.GetCompositeIdMapping();

                if (version != null)
                    classMapping.Version = version.GetVersionMapping();

                if (discriminator != null)
                    classMapping.Discriminator = ((IDiscriminatorMappingProvider)discriminator).GetDiscriminatorMapping();

                if (Cache.IsDirty)
                    classMapping.Cache = ((ICacheMappingProvider)Cache).GetCacheMapping();

                classMapping.Tuplizer = tuplizerMapping;
            }

            foreach (var property in Properties)
                mapping.AddOrReplaceProperty(property.GetPropertyMapping());

            foreach (var collection in collections)
                mapping.AddOrReplaceCollection(collection.GetCollectionMapping());

            foreach (var component in Components)
                mapping.AddOrReplaceComponent(component.GetComponentMapping());

            foreach (var oneToOne in oneToOnes)
                mapping.AddOrReplaceOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var reference in references)
                mapping.AddOrReplaceReference(reference.GetManyToOneMapping());

            foreach (var any in anys)
                mapping.AddOrReplaceAny(any.GetAnyMapping());

            foreach (var storedProcedure in storedProcedures)
                mapping.AddStoredProcedure(storedProcedure.GetStoredProcedureMapping());
        }

        protected override OneToManyPart<TChild> HasMany<TChild>(Member property)
        {
            mappedMembers.Add(property);
            return base.HasMany<TChild>(property);
        }

        public void IgnoreProperty(Expression<Func<T, object>> expression)
        {
            mappedMembers.Add(expression.ToMember());
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperty(string name)
        {
            ((IPropertyIgnorer)this).IgnoreProperties(name);

            return this;
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperties(string first, params string[] others)
        {
            var options = (others ?? new string[0]).Concat(new[] { first }).ToArray();

            ((IPropertyIgnorer)this).IgnoreProperties(x => x.Name.In(options));

            return this;
        }

        IPropertyIgnorer IPropertyIgnorer.IgnoreProperties(Func<Member, bool> predicate)
        {
            typeof(T).GetProperties()
                .Select(x => x.ToMember())
                .Where(predicate)
                .Each(mappedMembers.Add);

            return this;
        }

        public override IdentityPart Id(Expression<Func<T, object>> expression)
        {
            mappedMembers.Add(expression.ToMember());
            return base.Id(expression);
        }

        public override CompositeIdentityPart<T> CompositeId()
        {
            var part = new AutoCompositeIdentityPart<T>(mappedMembers);

            compositeId = part;

            return part;
        }

        protected override PropertyPart Map(Member property, string columnName)
        {
            mappedMembers.Add(property);
            return base.Map(property, columnName);
        }

        protected override ManyToOnePart<TOther> References<TOther>(Member property, string columnName)
        {
            mappedMembers.Add(property);
            return base.References<TOther>(property, columnName);
        }

        protected override ManyToManyPart<TChild> HasManyToMany<TChild>(Member property)
        {
            mappedMembers.Add(property);
            return base.HasManyToMany<TChild>(property);
        }

        protected override ComponentPart<TComponent> Component<TComponent>(Member property, Action<ComponentPart<TComponent>> action)
        {
            mappedMembers.Add(property);

            if (action == null)
                action = c => { };

            return base.Component(property, action);
        }

        public override IdentityPart Id(Expression<Func<T, object>> expression, string column)
        {
            mappedMembers.Add(expression.ToMember());
            return base.Id(expression, column);
        }

        protected override OneToOnePart<TOther> HasOne<TOther>(Member property)
        {
            mappedMembers.Add(property);
            return base.HasOne<TOther>(property);
        }

        protected override VersionPart Version(Member property)
        {
            mappedMembers.Add(property);
            return base.Version(property);
        }

		public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn, Action<AutoJoinedSubClassPart<TSubclass>> action)
			where TSubclass : T
        {
            var genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var joinedclass = (AutoJoinedSubClassPart<TSubclass>)Activator.CreateInstance(genericType, keyColumn);

            if (action != null)
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
            
            if (action != null)
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