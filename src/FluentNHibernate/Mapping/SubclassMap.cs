using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Defines a mapping for an entity subclass. Derive from this class to create a mapping,
    /// and use the constructor to control how your entity is persisted.
    /// </summary>
    /// <example>
    /// public class EmployeeMap : SubclassMap&lt;Employee&gt;
    /// {
    ///   public EmployeeMap()
    ///   {
    ///     Map(x => x.Name);
    ///     Map(x => x.Age);
    ///   }
    /// }
    /// </example>
    /// <typeparam name="T">Entity type to map</typeparam>
    public class SubclassMap<T> : ClasslikeMapBase<T>, IIndeterminateSubclassMappingProvider
    {
        private readonly MappingProviderStore providers;
        readonly AttributeStore<SubclassMapping> attributes = new AttributeStore<SubclassMapping>();

        // this is a bit weird, but we need a way of delaying the generation of the subclass mappings until we know
        // what the parent subclass type is...
        readonly IDictionary<Type, IIndeterminateSubclassMappingProvider> indetermineateSubclasses = new Dictionary<Type, IIndeterminateSubclassMappingProvider>();
        bool nextBool = true;
        readonly IList<JoinMapping> joins = new List<JoinMapping>();

        public SubclassMap()
            : this(new MappingProviderStore())
        {}

        protected SubclassMap(MappingProviderStore providers)
            : base(providers)
        {
            this.providers = providers;
        }

        /// <summary>
        /// Inverts the next boolean setting
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public SubclassMap<T> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        /// <summary>
        /// (optional) Specifies that this subclass is abstract
        /// </summary>
        public void Abstract()
        {
            attributes.Set(x => x.Abstract, nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Sets the dynamic insert behaviour
        /// </summary>
        public void DynamicInsert()
        {
            attributes.Set(x => x.DynamicInsert, nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Sets the dynamic update behaviour
        /// </summary>
        public void DynamicUpdate()
        {
            attributes.Set(x => x.DynamicUpdate, nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Specifies that this entity should be lazy loaded
        /// </summary>
        public void LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Specify a proxy type for this entity
        /// </summary>
        /// <typeparam name="TProxy">Proxy type</typeparam>
        public void Proxy<TProxy>()
        {
            Proxy(typeof(TProxy));
        }

        /// <summary>
        /// Specify a proxy type for this entity
        /// </summary>
        /// <param name="proxyType">Proxy type</param>
        public void Proxy(Type proxyType)
        {
            attributes.Set(x => x.Proxy, proxyType.AssemblyQualifiedName);
        }

        /// <summary>
        /// Specify that a select should be performed before an update of this entity
        /// </summary>
        public void SelectBeforeUpdate()
        {
            attributes.Set(x => x.SelectBeforeUpdate, nextBool);
            nextBool = true;
        }

        [Obsolete("Use a new SubclassMap")]
        public void Subclass<TSubclass>(Action<SubclassMap<TSubclass>> subclassDefinition)
        {
            var subclass = new SubclassMap<TSubclass>();

            subclassDefinition(subclass);

            indetermineateSubclasses[typeof(TSubclass)] = subclass;
        }

        /// <summary>
        /// Set the discriminator value, if this entity is in a table-per-class-hierarchy
        /// mapping strategy.
        /// </summary>
        /// <param name="discriminatorValue">Discriminator value</param>
        public void DiscriminatorValue(object discriminatorValue)
        {
            attributes.Set(x => x.DiscriminatorValue, discriminatorValue);
        }

        /// <summary>
        /// Sets the table name
        /// </summary>
        /// <param name="table">Table name</param>
        public void Table(string table)
        {
            attributes.Set(x => x.TableName, table);
        }

        /// <summary>
        /// Sets the schema
        /// </summary>
        /// <param name="schema">Schema</param>
        public void Schema(string schema)
        {
            attributes.Set(x => x.Schema, schema);
        }

        /// <summary>
        /// Specifies a check constraint
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        public void Check(string constraint)
        {
            attributes.Set(x => x.Check, constraint);
        }

        /// <summary>
        /// Adds a column to the key for this subclass, if used
        /// in a table-per-subclass strategy.
        /// </summary>
        /// <param name="column">Column name</param>
        public void KeyColumn(string column)
        {
            KeyMapping key;

            if (attributes.IsSpecified(x => x.Key))
                key = attributes.Get(x => x.Key);
            else
                key = new KeyMapping();

            key.AddColumn(new ColumnMapping { Name = column });

            attributes.Set(x => x.Key, key);
        }

        /// <summary>
        /// Subselect query
        /// </summary>
        /// <param name="subselect">Subselect query</param>
        public void Subselect(string subselect)
        {
            attributes.Set(x => x.Subselect, subselect);
        }

        /// <summary>
        /// Specifies a persister for this entity
        /// </summary>
        /// <typeparam name="TPersister">Persister type</typeparam>
        public void Persister<TPersister>()
        {
            attributes.Set(x => x.Persister, new TypeReference(typeof(TPersister)));
        }

        /// <summary>
        /// Specifies a persister for this entity
        /// </summary>
        /// <param name="type">Persister type</param>
        public void Persister(Type type)
        {
            attributes.Set(x => x.Persister, new TypeReference(type));
        }

        /// <summary>
        /// Specifies a persister for this entity
        /// </summary>
        /// <param name="type">Persister type</param>
        public void Persister(string type)
        {
            attributes.Set(x => x.Persister, new TypeReference(type));
        }

        /// <summary>
        /// Set the query batch size
        /// </summary>
        /// <param name="batchSize">Batch size</param>
        public void BatchSize(int batchSize)
        {
            attributes.Set(x => x.BatchSize, batchSize);
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public void EntityName(string entityname)
        {
            attributes.Set(x => x.EntityName, entityname);
        }

        /// <summary>
        /// Links this entity to another table, to create a composite entity from two or
        /// more tables. This only works if you're in a table-per-inheritance-hierarchy
        /// strategy.
        /// </summary>
        /// <param name="tableName">Joined table name</param>
        /// <param name="action">Joined table mapping</param>
        /// <example>
        /// Join("another_table", join =>
        /// {
        ///   join.Map(x => x.Name);
        ///   join.Map(x => x.Age);
        /// });
        /// </example>
        public void Join(string tableName, Action<JoinPart<T>> action)
        {
            var join = new JoinPart<T>(tableName);

            action(join);

            joins.Add(((IJoinMappingProvider)join).GetJoinMapping());
        }

        /// <summary>
        /// (optional) Specifies the entity from which this subclass descends/extends.
        /// </summary>
        /// <typeparam name="TOther">Type of the entity to extend</typeparam>
        public void Extends<TOther>()
        {
            Extends(typeof(TOther));
        }

        /// <summary>
        /// (optional) Specifies the entity from which this subclass descends/extends.
        /// </summary>
        /// <param name="type">Type of the entity to extend</param>
        public void Extends(Type type)
        {
            attributes.Set(x => x.Extends, type);
        }

        SubclassMapping IIndeterminateSubclassMappingProvider.GetSubclassMapping(SubclassType type)
        {
            var mapping = new SubclassMapping(type);

            GenerateNestedSubclasses(mapping);

            attributes.SetDefault(x => x.Type, typeof(T));
            attributes.SetDefault(x => x.Name, typeof(T).AssemblyQualifiedName);
            attributes.SetDefault(x => x.DiscriminatorValue, typeof(T).Name);

            // TODO: un-hardcode this
            var key = new KeyMapping();
            key.AddDefaultColumn(new ColumnMapping { Name = typeof(T).BaseType.Name + "_id" });

            attributes.SetDefault(x => x.TableName, GetDefaultTableName());
            attributes.SetDefault(x => x.Key, key);

            // TODO: this is nasty, we should find a better way
            mapping.OverrideAttributes(attributes.CloneInner());

            foreach (var join in joins)
                mapping.AddJoin(join);

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

            return mapping.DeepClone();
        }

        Type IIndeterminateSubclassMappingProvider.Extends
        {
            get { return attributes.Get(x => x.Extends); }
        }

        void GenerateNestedSubclasses(SubclassMapping mapping)
        {
            foreach (var subclassType in indetermineateSubclasses.Keys)
            {
                var subclassMapping = indetermineateSubclasses[subclassType].GetSubclassMapping(mapping.SubclassType);

                mapping.AddSubclass(subclassMapping);
            }
        }

        string GetDefaultTableName()
        {
#pragma warning disable 612,618
            var tableName = EntityType.Name;

            if (EntityType.IsGenericType)
            {
                // special case for generics: GenericType_GenericParameterType
                tableName = EntityType.Name.Substring(0, EntityType.Name.IndexOf('`'));

                foreach (var argument in EntityType.GetGenericArguments())
                {
                    tableName += "_";
                    tableName += argument.Name;
                }
            }
#pragma warning restore 612,618

            return "`" + tableName + "`";
        }
    }
}