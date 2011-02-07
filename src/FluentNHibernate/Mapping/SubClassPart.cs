using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class SubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, ISubclassMappingProvider
    {
        private readonly DiscriminatorPart parent;
        private readonly object discriminatorValue;
        private readonly MappingProviderStore providers;
        private readonly AttributeStore<SubclassMapping> attributes = new AttributeStore<SubclassMapping>();
        private readonly List<SubclassMapping> subclassMappings = new List<SubclassMapping>();
        private bool nextBool = true;

        public SubClassPart(DiscriminatorPart parent, object discriminatorValue)
            : this(parent, discriminatorValue, new MappingProviderStore())
        {}

        protected SubClassPart(DiscriminatorPart parent, object discriminatorValue, MappingProviderStore providers)
            : base(providers)
        {
            this.parent = parent;
            this.discriminatorValue = discriminatorValue;
            this.providers = providers;
        }

        SubclassMapping ISubclassMappingProvider.GetSubclassMapping()
        {
            var mapping = new SubclassMapping(SubclassType.Subclass, attributes.CloneInner());

            if (discriminatorValue != null)
                mapping.DiscriminatorValue = discriminatorValue;

            mapping.SetDefaultValue(x => x.Type, typeof(TSubclass));
            mapping.SetDefaultValue(x => x.Name, typeof(TSubclass).AssemblyQualifiedName);
            
            foreach (var property in providers.Properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in providers.Components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var oneToOne in providers.OneToOnes)
                mapping.AddOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var collection in providers.Collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            foreach (var reference in providers.References)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in providers.Anys)
                mapping.AddAny(any.GetAnyMapping());

            subclassMappings.Each(mapping.AddSubclass);

            return mapping;
        }

        public DiscriminatorPart SubClass<TChild>(object discriminatorValue, Action<SubClassPart<TChild>> action)
        {
            var subclass = new SubClassPart<TChild>(parent, discriminatorValue);

            action(subclass);

            subclassMappings.Add(((ISubclassMappingProvider)subclass).GetSubclassMapping());

            return parent;
        }

        public DiscriminatorPart SubClass<TChild>(Action<SubClassPart<TChild>> action)
        {
            return SubClass(null, action);
        }

        /// <summary>
        /// Sets whether this subclass is lazy loaded
        /// </summary>
        /// <returns></returns>
        public SubClassPart<TSubclass> LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> Proxy(Type type)
        {
            attributes.Set(x => x.Proxy, type.AssemblyQualifiedName);
            return this;
        }

        public SubClassPart<TSubclass> Proxy<T>()
        {
            return Proxy(typeof(T));
        }

        public SubClassPart<TSubclass> DynamicUpdate()
        {
            attributes.Set(x => x.DynamicUpdate, nextBool);
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> DynamicInsert()
        {
            attributes.Set(x => x.DynamicInsert, nextBool);
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> SelectBeforeUpdate()
        {
            attributes.Set(x => x.SelectBeforeUpdate, nextBool);
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> Abstract()
        {
            attributes.Set(x => x.Abstract, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public void EntityName(string entityName)
        {
            attributes.Set(x => x.EntityName, entityName);
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public SubClassPart<TSubclass> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }
    }
}
