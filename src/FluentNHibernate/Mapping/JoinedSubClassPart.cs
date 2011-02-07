using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class JoinedSubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, ISubclassMappingProvider
    {
        private readonly MappingProviderStore providers;
        private readonly ColumnMappingCollection<JoinedSubClassPart<TSubclass>> columns;
        private readonly List<SubclassMapping> subclassMappings = new List<SubclassMapping>();
        private readonly AttributeStore<SubclassMapping> attributes;
        private bool nextBool = true;

        public JoinedSubClassPart(string keyColumn)
            : this(keyColumn, new AttributeStore(), new MappingProviderStore())
        {}

        protected JoinedSubClassPart(string keyColumn, AttributeStore underlyingStore, MappingProviderStore providers)
            : base(providers)
        {
            this.providers = providers;
            attributes = new AttributeStore<SubclassMapping>(underlyingStore);
            columns = new ColumnMappingCollection<JoinedSubClassPart<TSubclass>>(this);
            columns.Add(keyColumn);
        }

        public virtual void JoinedSubClass<TNextSubclass>(string keyColumn, Action<JoinedSubClassPart<TNextSubclass>> action)
        {
            var subclass = new JoinedSubClassPart<TNextSubclass>(keyColumn);

            action(subclass);

            providers.Subclasses[typeof(TNextSubclass)] = subclass;

            subclassMappings.Add(((ISubclassMappingProvider)subclass).GetSubclassMapping());
        }

        public ColumnMappingCollection<JoinedSubClassPart<TSubclass>> KeyColumns
        {
            get { return columns; }
        }

        public JoinedSubClassPart<TSubclass> Table(string tableName)
        {
            attributes.Set(x => x.TableName, tableName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Schema(string schema)
        {
            attributes.Set(x => x.Schema, schema);
            return this;
        }

        public JoinedSubClassPart<TSubclass> CheckConstraint(string constraintName)
        {
            attributes.Set(x => x.Check, constraintName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy(Type type)
        {
            attributes.Set(x => x.Proxy, type.AssemblyQualifiedName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy<T>()
        {
            return Proxy(typeof(T));
        }

        public JoinedSubClassPart<TSubclass> LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> DynamicUpdate()
        {
            attributes.Set(x => x.DynamicUpdate, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> DynamicInsert()
        {
            attributes.Set(x => x.DynamicInsert, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> SelectBeforeUpdate()
        {
            attributes.Set(x => x.SelectBeforeUpdate, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> Abstract()
        {
            attributes.Set(x => x.Abstract, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public JoinedSubClassPart<TSubclass> EntityName(string entityName)
        {
            attributes.Set(x => x.EntityName, entityName);
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public JoinedSubClassPart<TSubclass> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        SubclassMapping ISubclassMappingProvider.GetSubclassMapping()
        {
            var mapping = new SubclassMapping(SubclassType.JoinedSubclass, attributes.CloneInner());

            mapping.Key = new KeyMapping { ContainingEntityType = typeof(TSubclass) };
            mapping.Name = typeof(TSubclass).AssemblyQualifiedName;
            mapping.Type = typeof(TSubclass);

            foreach (var column in columns)
                mapping.Key.AddColumn(column);

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

            return mapping;
        }
    }
}