using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    [Obsolete("REMOVE ME")]
    public class JoinedSubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, ISubclassMappingProvider
    {
        readonly MappingProviderStore providers;
        readonly ColumnMappingCollection<JoinedSubClassPart<TSubclass>> columns;
        readonly List<SubclassMapping> subclassMappings = new List<SubclassMapping>();
        readonly AttributeStore attributes;
        bool nextBool = true;

        public JoinedSubClassPart(string keyColumn)
            : this(keyColumn, new AttributeStore(), new MappingProviderStore())
        {}

        protected JoinedSubClassPart(string keyColumn, AttributeStore attributes, MappingProviderStore providers)
            : base(providers)
        {
            this.providers = providers;
            this.attributes = attributes;
            columns = new ColumnMappingCollection<JoinedSubClassPart<TSubclass>>(this)
            {
                keyColumn
            };
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
            attributes.Set("TableName", Layer.UserSupplied, tableName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Schema(string schema)
        {
            attributes.Set("Schema", Layer.UserSupplied, schema);
            return this;
        }

        public JoinedSubClassPart<TSubclass> CheckConstraint(string constraintName)
        {
            attributes.Set("Check", Layer.UserSupplied, constraintName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy(Type type)
        {
            attributes.Set("Proxy", Layer.UserSupplied, type.AssemblyQualifiedName);
            return this;
        }

        public JoinedSubClassPart<TSubclass> Proxy<T>()
        {
            return Proxy(typeof(T));
        }

        public JoinedSubClassPart<TSubclass> LazyLoad()
        {
            attributes.Set("Lazy", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> DynamicUpdate()
        {
            attributes.Set("DynamicUpdate", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> DynamicInsert()
        {
            attributes.Set("DynamicInsert", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> SelectBeforeUpdate()
        {
            attributes.Set("SelectBeforeUpdate", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        public JoinedSubClassPart<TSubclass> Abstract()
        {
            attributes.Set("Abstract", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public JoinedSubClassPart<TSubclass> EntityName(string entityName)
        {
            attributes.Set("EntityName", Layer.UserSupplied, entityName);
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
            var mapping = new SubclassMapping(SubclassType.JoinedSubclass, attributes.Clone());

            mapping.Set(x => x.Key, Layer.Defaults, new KeyMapping { ContainingEntityType = typeof(TSubclass) });
            mapping.Set(x => x.Name, Layer.Defaults, typeof(TSubclass).AssemblyQualifiedName);
            mapping.Set(x => x.Type, Layer.Defaults, typeof(TSubclass));

            foreach (var column in columns)
                mapping.Key.AddColumn(Layer.Defaults, column);

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