using System;
using System.Diagnostics;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping.Builders
{
    public class MapBuilder<TKey, TValue>
    {
        readonly MapMapping mapping;
        KeyMapping key;
        IndexMapping index;
        CompositeIndexMapping compositeIndex;
        ManyToManyMapping manyToMany;
        OneToManyMapping oneToMany;
        ElementMapping element;
        bool nextBool = true;

        public MapBuilder(MapMapping mapping, Member member)
        {
            this.mapping = mapping;

            InitialiseDefaults(member);
        }

        void InitialiseDefaults(Member member)
        {
            mapping.Member = member;
            mapping.SetDefaultValue(x => x.Name, member.Name);
            mapping.SetDefaultValue(x => x.TableName, mapping.ContainingEntityType.Name + member.Name);

            mapping.Key = key = new KeyMapping();
            key.AddDefaultColumn(new ColumnMapping { Name = mapping.ContainingEntityType.Name + "_id" });

            mapping.Index = index = new IndexMapping();
            mapping.Index.As<IndexMapping>(ix =>
                ix.SetDefaultValue(x => x.Type, new TypeReference(KeyType)));

            if (ValueType.IsSimpleType())
            {
                // value type value (element)
                mapping.Element = element = new ElementMapping();
                element.AddDefaultColumn(new ColumnMapping {Name = "Value"});
                element.SetDefaultValue(x => x.Type, new TypeReference(typeof(TValue)));
            }
            else
            {
                // entity value
                mapping.Relationship = manyToMany = new ManyToManyMapping();
                manyToMany.Class = new TypeReference(ValueType);
                manyToMany.AddDefaultColumn(new ColumnMapping { Name = ValueType.Name + "_id" });
                manyToMany.ParentType = mapping.ContainingEntityType;
                manyToMany.ChildType = ValueType;
            }

            if (KeyType.IsSimpleType())
            {
                mapping.Index.As<IndexMapping>(ix =>
                    ix.AddDefaultColumn(new ColumnMapping { Name = "Key" }));
            }
            else
            {
                mapping.Index.As<IndexMapping>(ix =>
                {
                    ix.IsManyToMany = true;
                    ix.AddDefaultColumn(new ColumnMapping { Name = KeyType.Name + "_id" });
                });
            }
        }

        /// <summary>
        /// Inverts the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public MapBuilder<TKey, TValue> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        /// <summary>
        /// Specify the access strategy for the map property
        /// </summary>
        public AccessStrategyBuilder<MapBuilder<TKey, TValue>> Access
        {
            get { return new AccessStrategyBuilder<MapBuilder<TKey, TValue>>(this, value => mapping.Access = value); }
        }

        /// <summary>
        /// Specify the cascade strategy for the collection
        /// </summary>
        public CollectionCascadeExpression<MapBuilder<TKey, TValue>> Cascade
        {
            get { return new CollectionCascadeExpression<MapBuilder<TKey, TValue>>(this, value => mapping.Cascade = value); }
        }

        /// <summary>
        /// Specify the cascade strategy for the collection
        /// </summary>
        public FetchTypeExpression<MapBuilder<TKey, TValue>> Fetch
        {
            get { return new FetchTypeExpression<MapBuilder<TKey, TValue>>(this, value => mapping.Fetch = value); }
        }

        /// <summary>
        /// Sets the map name. Optional.
        /// </summary>
        /// <param name="mapName">Map name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Name(string mapName)
        {
            mapping.Name = mapName;
            return this;
        }

        /// <summary>
        /// Sets the name of the table containing the dictionary values. Optional.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Table(string tableName)
        {
            mapping.TableName = tableName;
            return this;
        }

        /// <summary>
        /// Sets the name of the schema containing the map table.
        /// </summary>
        /// <param name="schema">Schema name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Schema(string schema)
        {
            mapping.Schema = schema;
            return this;
        }

        /// <summary>
        /// Sets the name of the catalog containing the map table.
        /// </summary>
        /// <param name="catalog">Catalog name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Catalog(string catalog)
        {
            mapping.Catalog = catalog;
            return this;
        }

        /// <summary>
        /// Specifies whether this collection is lazy
        /// </summary>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Lazy()
        {
            mapping.Lazy = nextBool ? MappingModel.Collections.Lazy.True : MappingModel.Collections.Lazy.False;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies whether this collection is extra lazy
        /// </summary>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ExtraLazy()
        {
            mapping.Lazy = MappingModel.Collections.Lazy.Extra;
            return this;
        }

        /// <summary>
        /// Specifies whether this collection is inverse
        /// </summary>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Inverse()
        {
            mapping.Inverse = nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies whether this collection is read-only (mutable=false)
        /// </summary>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ReadOnly()
        {
            mapping.Mutable = !nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Sets the order by clause
        /// </summary>
        /// <param name="clause">Order by</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> OrderBy(string clause)
        {
            mapping.OrderBy = clause;
            return this;
        }

        /// <summary>
        /// Sets the where clause
        /// </summary>
        /// <param name="clause">Order by</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Where(string clause)
        {
            mapping.Where = clause;
            return this;
        }

        /// <summary>
        /// Sets the batch size
        /// </summary>
        /// <param name="batchSize">Batch size</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> BatchSize(int batchSize)
        {
            mapping.BatchSize = batchSize;
            return this;
        }

        /// <summary>
        /// Sets the collection persister
        /// </summary>
        /// <typeparam name="TPersister">Persister type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Persister<TPersister>()
        {
            return Persister(typeof(TPersister));
        }

        /// <summary>
        /// Sets the collection persister
        /// </summary>
        /// <param name="persister">Persister type</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Persister(Type persister)
        {
            mapping.Persister = new TypeReference(persister);
            return this;
        }

        /// <summary>
        /// Sets the collection persister
        /// </summary>
        /// <param name="persister">Persister type</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Persister(string persister)
        {
            mapping.Persister = new TypeReference(persister);
            return this;
        }

        /// <summary>
        /// Sets the collection type
        /// </summary>
        /// <typeparam name="TCollection">Collection type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> CollectionType<TCollection>()
        {
            return CollectionType(typeof(TCollection));
        }

        /// <summary>
        /// Sets the collection type
        /// </summary>
        /// <param name="collectionType">Collection type</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> CollectionType(Type collectionType)
        {
            mapping.CollectionType = new TypeReference(collectionType);
            return this;
        }

        /// <summary>
        /// Sets the collection type
        /// </summary>
        /// <param name="collectionType">Collection type</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> CollectionType(string collectionType)
        {
            mapping.CollectionType = new TypeReference(collectionType);
            return this;
        }

        /// <summary>
        /// Sets the check constraint name
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Check(string constraint)
        {
            mapping.Check = constraint;
            return this;
        }

        /// <summary>
        /// Specifies whether this collection is optimistically locked
        /// </summary>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> OptimisticLock()
        {
            mapping.OptimisticLock = nextBool.ToString();
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Sets the subselect query
        /// </summary>
        /// <param name="query">Subselect query</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Subselect(string query)
        {
            mapping.Subselect = query;
            return this;
        }

        /// <summary>
        /// Configure the caching of this collection
        /// </summary>
        /// <param name="cacheConfiguration">Cache configuration</param>
        public MapBuilder<TKey, TValue> Cache(Action<CacheBuilder> cacheConfiguration)
        {
            mapping.Cache = mapping.Cache ?? new CacheMapping();

            cacheConfiguration(new CacheBuilder(mapping.Cache, mapping.ContainingEntityType));

            return this;
        }

        /// <overloads>
        /// Applies a filter to this entity given it's name.
        /// </overloads>
        /// <summary>
        /// Applies a filter to this entity given it's name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        /// <param name="condition">The condition to apply</param>
        public MapBuilder<TKey, TValue> ApplyFilter(string name, string condition)
        {
            var filterMapping = new FilterMapping();
            var builder = new FilterBuilder(filterMapping);
            
            builder.Name(name);
            builder.Condition(condition);

            mapping.Filters.Add(filterMapping);
            return this;
        }

        /// <overloads>
        /// Applies a filter to this entity given it's name.
        /// </overloads>
        /// <summary>
        /// Applies a filter to this entity given it's name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        public MapBuilder<TKey, TValue> ApplyFilter(string name)
        {
            return ApplyFilter(name, null);
        }

        /// <overloads>
        /// Applies a named filter to this one-to-many.
        /// </overloads>
        /// <summary>
        /// Applies a named filter to this one-to-many.
        /// </summary>
        /// <param name="condition">The condition to apply</param>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        public MapBuilder<TKey, TValue> ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
        {
            return ApplyFilter(new TFilter().Name, condition);
        }

        /// <summary>
        /// Applies a named filter to this one-to-many.
        /// </summary>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        public MapBuilder<TKey, TValue> ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
        {
            return ApplyFilter<TFilter>(null);
        }

        /// <summary>
        /// Specify how the index (or key) is configured.
        /// </summary>
        /// <param name="indexConfiguration">Configuration <see cref="Action"/></param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> DictionaryKey(Action<IndexBuilder> indexConfiguration)
        {
            if (!(mapping.Index is IndexMapping))
                mapping.Index = index;

            indexConfiguration(new IndexBuilder(index));
            return this;
        }

        /// <summary>
        /// Specifies the name of the column used for the keys of the dictionary, and the
        /// type of the key.
        /// </summary>
        /// <param name="indexColumnName">Key column name</param>
        /// <typeparam name="TIndexType">Key type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> DictionaryKey<TIndexType>(string indexColumnName)
        {
            return DictionaryKey(ix =>
            {
                ix.Column(indexColumnName);
                ix.Type<TIndexType>();
            });
        }

        /// <summary>
        /// Specifies the name of the column used for the keys of the dictionary, while using
        /// the type inferred from the dictionary signature.
        /// </summary>
        /// <param name="indexColumnName">Key column name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> DictionaryKey(string indexColumnName)
        {
            return DictionaryKey(ix => ix.Column(indexColumnName));
        }

        /// <summary>
        /// Specifies the type of the keys used for the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <typeparam name="TIndexType">Key type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> DictionaryKey<TIndexType>()
        {
            return DictionaryKey(ix => ix.Type<TIndexType>());
        }

        /// <summary>
        /// Specify how the element (or value) is configured.
        /// </summary>
        /// <param name="elementConfiguration">Configuration <see cref="Action"/></param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> DictionaryValue(Action<ElementBuilder> elementConfiguration)
        {
            mapping.Element = element = element ?? new ElementMapping();
            mapping.Relationship = null;
            elementConfiguration(new ElementBuilder(element));
            return this;
        }

        /// <summary>
        /// Specifies the name of the column used for the values of the dictionary, and the
        /// type of the values.
        /// </summary>
        /// <param name="elementColumnName">Value column name</param>
        /// <typeparam name="TElementType">Value type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> DictionaryValue<TElementType>(string elementColumnName)
        {
            return DictionaryValue(el =>
            {
                el.Column(elementColumnName);
                el.Type<TElementType>();
            });
        }

        /// <summary>
        /// Specifies the name of the column used for the values of the dictionary, while using
        /// the type inferred from the dictionary signature.
        /// </summary>
        /// <param name="elementColumnName">Value column name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> DictionaryValue(string elementColumnName)
        {
            return DictionaryValue(el => el.Column(elementColumnName));
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <typeparam name="TElementType">Value type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> DictionaryValue<TElementType>()
        {
            return DictionaryValue(el => el.Type<TElementType>());
        }

        /// <summary>
        /// Specify how the foreign key is configured.
        /// </summary>
        /// <param name="keyConfiguration">Configuration <see cref="Action"/></param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ForeignKey(Action<KeyBuilder> keyConfiguration)
        {
            mapping.Key = key = key ?? new KeyMapping();
            keyConfiguration(new KeyBuilder(key));
            return this;
        }

        /// <summary>
        /// Specifies the name of the column used for the foreign key.
        /// </summary>
        /// <param name="keyColumnName">Key column name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ForeignKey(string keyColumnName)
        {
            return ForeignKey(ke => ke.Column(keyColumnName));
        }

        /// <summary>
        /// Define a component (composite-element) to be used as the value of the dictionary.
        /// </summary>
        /// <param name="componentBuilder">Builder action</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> Component(Action<CompositeElementBuilder<TValue>> componentBuilder)
        {
            var compositeElementMapping = new CompositeElementMapping();
            var builder = new CompositeElementBuilder<TValue>(compositeElementMapping, mapping.ContainingEntityType);

            componentBuilder(builder);

            mapping.CompositeElement = compositeElementMapping;
            mapping.Element = null;
            mapping.Relationship = null;
                
            return this;
        }

        /// <summary>
        /// Define a component for use as the index (composite-index) or dictionary key.
        /// </summary>
        /// <param name="componentBuilder">Builder action</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ComponentIndex(Action<CompositeIndexBuilder<TKey>> componentBuilder)
        {
            if (!(mapping.Index is CompositeIndexMapping))
                mapping.Index = compositeIndex = compositeIndex ?? new CompositeIndexMapping();

            componentBuilder(new CompositeIndexBuilder<TKey>(compositeIndex));

            return this;
        }

        /// <summary>
        /// Specify the sorting for the dictionary.
        /// </summary>
        public SortBuilder<MapBuilder<TKey, TValue>> Sort
        {
            get { return new SortBuilder<MapBuilder<TKey,TValue>>(this, value => mapping.Sort = value); }
        }

        /// <summary>
        /// Specify the relationship is a one-to-many, this implies the key and value columns of the
        /// dictionary will be stored in the child table.
        /// </summary>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> OneToMany()
        {
            return OneToMany(om => {});
        }

        /// <summary>
        /// Specify the relationship is a one-to-many, this implies the key and value columns of the
        /// dictionary will be stored in the child table.
        /// </summary>
        /// <param name="relationshipConfiguration">Builder for one-to-many</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> OneToMany(Action<OneToManyBuilder> relationshipConfiguration)
        {
            mapping.Element = null;
            mapping.CompositeElement = null;
            mapping.Relationship = oneToMany = oneToMany ?? new OneToManyMapping();
            mapping.Relationship.As<OneToManyMapping>(re =>
            {
                re.Class = new TypeReference(ValueType);
                re.ChildType = ValueType;
            });
            relationshipConfiguration(new OneToManyBuilder(oneToMany));
            return this;
        }

        /// <summary>
        /// Specify this relationship as a one-to-many of <typeparamref name="TChild"/>
        /// </summary>
        /// <typeparam name="TChild">Child type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> OneToMany<TChild>()
        {
            return OneToMany(om => om.Type<TChild>());
        }

        /// <summary>
        /// Specify the relationship is a many-to-many, this implies that the key and value columns of the
        /// dictionary will be stored in a separate table.
        /// </summary>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ManyToMany()
        {
            ManyToMany(mm => {});
            return this;
        }

        /// <summary>
        /// Specify the relationship is a many-to-many, this implies that the key and value columns of the
        /// dictionary will be stored in a separate table.
        /// </summary>
        /// <param name="relationshipConfiguration">Builder for many-to-many</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ManyToMany(Action<ManyToManyBuilder> relationshipConfiguration)
        {
            mapping.Element = null;
            mapping.CompositeElement = null;
            mapping.Relationship = manyToMany;
            relationshipConfiguration(new ManyToManyBuilder(manyToMany));
            return this;
        }

        /// <summary>
        /// Specify this relationship as a many-to-many with the <paramref name="relationshipColumn"/> column name,
        /// and type of <typeparamref name="TChild" />
        /// </summary>
        /// <param name="relationshipColumn">Column name</param>
        /// <typeparam name="TChild">Child type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ManyToMany<TChild>(string relationshipColumn)
        {
            return ManyToMany(mm =>
            {
                mm.Column(relationshipColumn);
                mm.Type<TChild>();
            });
        }

        /// <summary>
        /// Specify this relationship as a many-to-many with the <paramref name="relationshipColumn"/> column name
        /// </summary>
        /// <param name="relationshipColumn">Column name</param>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ManyToMany(string relationshipColumn)
        {
            return ManyToMany(mm => mm.Column(relationshipColumn));
        }

        /// <summary>
        /// Specify this relationship as a many-to-many of <typeparamref name="TChild"/>
        /// </summary>
        /// <typeparam name="TChild">Child type</typeparam>
        /// <returns>Builder</returns>
        public MapBuilder<TKey, TValue> ManyToMany<TChild>()
        {
            return ManyToMany(mm => mm.Type<TChild>());
        }

        static Type KeyType
        {
            get { return typeof(TKey); }
        }

        static Type ValueType
        {
            get { return typeof(TValue); }
        }

        #region obsolete members

        [Obsolete("This bugger does nothing now")]
        public MapBuilder<TKey, TValue> AsEntityMap()
        {
            return this;
        }

        #endregion
    }
}