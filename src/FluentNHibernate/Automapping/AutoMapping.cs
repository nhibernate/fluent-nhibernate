using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class AutoMapping<T> : ClassMap<T>, IAutoClasslike, IPropertyIgnorer
    {
        readonly MappingProviderStore providers;
        readonly IList<Member> mappedMembers;

        public AutoMapping(IList<Member> mappedMembers)
            : this(mappedMembers, new AttributeStore(), new MappingProviderStore())
        {}

        AutoMapping(IList<Member> mappedMembers, AttributeStore attributes, MappingProviderStore providers)
            : base(attributes, providers)
        {
            this.mappedMembers = mappedMembers;
            this.providers = providers;
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
            mapping.MergeAttributes(attributes.Clone());

            if (mapping is ClassMapping)
            {
                var classMapping = (ClassMapping)mapping;

                if (providers.Id != null)
                    classMapping.Set(x => x.Id, Layer.Defaults, providers.Id.GetIdentityMapping());

                if (providers.NaturalId != null)
                    classMapping.Set(x => x.NaturalId, Layer.Defaults, providers.NaturalId.GetNaturalIdMapping());

                if (providers.CompositeId != null)
                    classMapping.Set(x => x.Id, Layer.Defaults, providers.CompositeId.GetCompositeIdMapping());

                if (providers.Version != null)
                    classMapping.Set(x => x.Version, Layer.Defaults, providers.Version.GetVersionMapping());

                if (providers.Discriminator != null)
                    classMapping.Set(x => x.Discriminator, Layer.Defaults, providers.Discriminator.GetDiscriminatorMapping());

                if (Cache.IsDirty)
                    classMapping.Set(x => x.Cache, Layer.Defaults, ((ICacheMappingProvider)Cache).GetCacheMapping());

                foreach (var join in providers.Joins)
                    classMapping.AddJoin(join.GetJoinMapping());

                classMapping.Set(x => x.Tuplizer, Layer.Defaults, providers.TuplizerMapping);
            }

            foreach (var property in providers.Properties)
                mapping.AddOrReplaceProperty(property.GetPropertyMapping());

            foreach (var collection in providers.Collections)
                mapping.AddOrReplaceCollection(collection.GetCollectionMapping());

            foreach (var component in providers.Components)
                mapping.AddOrReplaceComponent(component.GetComponentMapping());

            foreach (var oneToOne in providers.OneToOnes)
                mapping.AddOrReplaceOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var reference in providers.References)
                mapping.AddOrReplaceReference(reference.GetManyToOneMapping());

            foreach (var any in providers.Anys)
                mapping.AddOrReplaceAny(any.GetAnyMapping());

            foreach (var storedProcedure in providers.StoredProcedures)
                mapping.AddStoredProcedure(storedProcedure.GetStoredProcedureMapping());

            foreach (var filter in providers.Filters)
                mapping.AddOrReplaceFilter(filter.GetFilterMapping());
        }

        internal override void OnMemberMapped(Member member)
        {
            mappedMembers.Add(member);
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

		public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn, Action<AutoJoinedSubClassPart<TSubclass>> action)
			where TSubclass : T
        {
            var genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var joinedclass = (AutoJoinedSubClassPart<TSubclass>)Activator.CreateInstance(genericType, keyColumn);

            if (action != null)
                action(joinedclass);

            providers.Subclasses[typeof(TSubclass)] = joinedclass;

		    return joinedclass;
        }

        public IAutoClasslike JoinedSubClass(Type type, string keyColumn)
        {
            var genericType = typeof (AutoJoinedSubClassPart<>).MakeGenericType(type);
            var joinedclass = (ISubclassMappingProvider)Activator.CreateInstance(genericType, keyColumn);

            // remove any mappings for the same type, then re-add
            providers.Subclasses[type] = joinedclass;

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
            providers.Subclasses[typeof(TSubclass)] = subclass;

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
            providers.Subclasses[type] = subclass;

            return (IAutoClasslike)subclass;
        }
        
        // hide the base one D:
        private new void Join(string table, Action<JoinPart<T>> action)
        { }

        public void Join(string table, Action<AutoJoinPart<T>> action)
        {
            var join = new AutoJoinPart<T>(mappedMembers, table);

            action(join);

            providers.Joins.Add(join);
        }

#pragma warning disable 809
        // hide this - imports aren't supported in overrides
        [Obsolete("Imports aren't supported in overrides.", true)]
        public new ImportPart ImportType<TImport>()
        {
            return null;
        }
#pragma warning restore 809
    }
}