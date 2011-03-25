using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping
{
    public class ManyToManyPart<TChild> : ToManyBase<ManyToManyPart<TChild>, TChild, ManyToManyMapping>
    {
        private readonly IList<FilterPart> childFilters = new List<FilterPart>();
        private readonly Type entity;
        private readonly FetchTypeExpression<ManyToManyPart<TChild>> fetch;
        private readonly NotFoundExpression<ManyToManyPart<TChild>> notFound;
        private IndexManyToManyPart manyToManyIndex;
        private IndexPart index;
        private readonly ColumnMappingCollection<ManyToManyPart<TChild>> childKeyColumns;
        private readonly ColumnMappingCollection<ManyToManyPart<TChild>> parentKeyColumns;
        private readonly Type childType;
        private Type valueType;
        private bool isTernary;

        public ManyToManyPart(Type entity, Member property)
            : this(entity, property, property.PropertyType)
        {
            childType = property.PropertyType;
        }

        protected ManyToManyPart(Type entity, Member member, Type collectionType)
            : base(entity, member, collectionType)
        {
            this.entity = entity;
            childType = collectionType;

            fetch = new FetchTypeExpression<ManyToManyPart<TChild>>(this, value => collectionAttributes.Set(x => x.Fetch, value));
            notFound = new NotFoundExpression<ManyToManyPart<TChild>>(this, value => relationshipAttributes.Set(x => x.NotFound, value));

            childKeyColumns = new ColumnMappingCollection<ManyToManyPart<TChild>>(this);
            parentKeyColumns = new ColumnMappingCollection<ManyToManyPart<TChild>>(this);
        }

        /// <summary>
        /// Sets a single child key column. If there are multiple columns, use ChildKeyColumns.Add
        /// </summary>
        public ManyToManyPart<TChild> ChildKeyColumn(string childKeyColumn)
        {
            childKeyColumns.Clear(); 
            childKeyColumns.Add(childKeyColumn);
            return this;
        }

        /// <summary>
        /// Sets a single parent key column. If there are multiple columns, use ParentKeyColumns.Add
        /// </summary>
        public ManyToManyPart<TChild> ParentKeyColumn(string parentKeyColumn)
        {
            parentKeyColumns.Clear(); 
            parentKeyColumns.Add(parentKeyColumn);
            return this;
        }

        public ColumnMappingCollection<ManyToManyPart<TChild>> ChildKeyColumns
        {
            get { return childKeyColumns; }
        }

        public ColumnMappingCollection<ManyToManyPart<TChild>> ParentKeyColumns
        {
            get { return parentKeyColumns; }
        }

        public ManyToManyPart<TChild> ForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName)
        {
            keyMapping.ForeignKey = parentForeignKeyName;
            relationshipAttributes.Set(x => x.ForeignKey, childForeignKeyName);
            return this;
        }

        public ManyToManyPart<TChild> ChildPropertyRef(string childPropertyRef)
        {
            relationshipAttributes.Set(x => x.ChildPropertyRef, childPropertyRef);
            return this;
        }

        public FetchTypeExpression<ManyToManyPart<TChild>> FetchType
        {
            get { return fetch; }
        }

        private void EnsureDictionary()
        {
            if (!typeof(IDictionary).IsAssignableFrom(childType))
                throw new ArgumentException(member.Name + " must be of type IDictionary to be used in a non-generic ternary association. Type was: " + childType);
        }

        private void EnsureGenericDictionary()
        {
            if (!(childType.IsGenericType && childType.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                throw new ArgumentException(member.Name + " must be of type IDictionary<> to be used in a ternary assocation. Type was: " + childType);
        }

        public ManyToManyPart<TChild> AsTernaryAssociation()
        {
            EnsureGenericDictionary();

            var indexType = typeof(TChild).GetGenericArguments()[0];
            var valueType = typeof(TChild).GetGenericArguments()[1];

            return AsTernaryAssociation(indexType.Name + "_id", valueType.Name + "_id");
        }

        public ManyToManyPart<TChild> AsTernaryAssociation(string indexColumn, string valueColumn)
        {
            return AsTernaryAssociation(indexColumn, valueColumn, x => {});
        }

        public ManyToManyPart<TChild> AsTernaryAssociation(string indexColumn, string valueColumn, Action<IndexManyToManyPart> indexAction)
        {
            EnsureGenericDictionary();

            var indexType = typeof(TChild).GetGenericArguments()[0];
            var valueType = typeof(TChild).GetGenericArguments()[1];

            manyToManyIndex = new IndexManyToManyPart(typeof(ManyToManyPart<TChild>));
            manyToManyIndex.Column(indexColumn);
            manyToManyIndex.Type(indexType);

            if (indexAction != null)
                indexAction(manyToManyIndex);

            ChildKeyColumn(valueColumn);
            this.valueType = valueType;

            isTernary = true;

            return this;
        }

        public ManyToManyPart<TChild> AsTernaryAssociation(Type indexType, Type valueType)
        {
            return AsTernaryAssociation(indexType, indexType.Name + "_id", valueType, valueType.Name + "_id");
        }

        public ManyToManyPart<TChild> AsTernaryAssociation(Type indexType, string indexColumn, Type valueType, string valueColumn)
        {
            return AsTernaryAssociation(indexType, indexColumn, valueType, valueColumn, x => {});
        }

        public ManyToManyPart<TChild> AsTernaryAssociation(Type indexType, string indexColumn, Type valueType, string valueColumn, Action<IndexManyToManyPart> indexAction)
        {
            EnsureDictionary();

            manyToManyIndex = new IndexManyToManyPart(typeof(ManyToManyPart<TChild>));
            manyToManyIndex.Column(indexColumn);
            manyToManyIndex.Type(indexType);

            if (indexAction != null)
                indexAction(manyToManyIndex);

            ChildKeyColumn(valueColumn);
            this.valueType = valueType;

            isTernary = true;

            return this;
        }

        public ManyToManyPart<TChild> AsSimpleAssociation()
        {
            EnsureGenericDictionary();

            var indexType = typeof(TChild).GetGenericArguments()[0];
            var valueType = typeof(TChild).GetGenericArguments()[1];

            return AsSimpleAssociation(indexType.Name + "_id", valueType.Name + "_id");
        }

        public ManyToManyPart<TChild> AsSimpleAssociation(string indexColumn, string valueColumn)
        {
            EnsureGenericDictionary();

            var indexType = typeof(TChild).GetGenericArguments()[0];
            var valueType = typeof(TChild).GetGenericArguments()[1];

            index = new IndexPart(indexType);
            index.Column(indexColumn);
            index.Type(indexType);

            ChildKeyColumn(valueColumn);
            this.valueType = valueType;

            isTernary = true;

            return this;
        }

        public ManyToManyPart<TChild> AsEntityMap()
        {
            // The argument to AsMap will be ignored as the ternary association will overwrite the index mapping for the map.
            // Therefore just pass null.
            return AsMap(null).AsTernaryAssociation();
        }

        public ManyToManyPart<TChild> AsEntityMap(string indexColumn, string valueColumn)
        {
            return AsMap(null).AsTernaryAssociation(indexColumn, valueColumn);
        }

        public Type ChildType
        {
            get { return typeof(TChild); }
        }

        public NotFoundExpression<ManyToManyPart<TChild>> NotFound
        {
            get { return notFound; }
        }

        protected override ICollectionRelationshipMapping GetRelationship()
        {
            var mapping = new ManyToManyMapping(relationshipAttributes.CloneInner())
            {
                ContainingEntityType = entity,

            };

            if (isTernary && valueType != null)
                mapping.Class = new TypeReference(valueType);

            foreach (var filterPart in childFilters)
                mapping.ChildFilters.Add(filterPart.GetFilterMapping());

            return mapping;
        }

        /// <summary>
        /// Sets the order-by clause on the collection element.
        /// </summary>
        public ManyToManyPart<TChild> OrderBy(string orderBy)
        {
            collectionAttributes.Set(x => x.OrderBy, orderBy);
            return this;
        }

        /// <summary>
        /// Sets the order-by clause on the many-to-many element.
        /// </summary>
        public ManyToManyPart<TChild> ChildOrderBy(string orderBy)
        {
            relationshipAttributes.Set(x => x.OrderBy, orderBy);
            return this;
        }

        public ManyToManyPart<TChild> ReadOnly()
        {            
            collectionAttributes.Set(x => x.Mutable, !nextBool);
            nextBool = true;
            return this;
        }

        public ManyToManyPart<TChild> Subselect(string subselect)
        {
            collectionAttributes.Set(x => x.Subselect, subselect);
            return this;
        }

        /// <overloads>
        /// Applies a filter to the child element of this entity given it's name.
        /// </overloads>
        /// <summary>
        /// Applies a filter to the child element of this entity given it's name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        /// <param name="condition">The condition to apply</param>
        public ManyToManyPart<TChild> ApplyChildFilter(string name, string condition)
        {
            var part = new FilterPart(name, condition);
            childFilters.Add(part);
            return this;
        }

        /// <overloads>
        /// Applies a filter to the child element of this entity given it's name.
        /// </overloads>
        /// <summary>
        /// Applies a filter to the child element of this entity given it's name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        public ManyToManyPart<TChild> ApplyChildFilter(string name)
        {
            return this.ApplyChildFilter(name, null);
        }

        /// <overloads>
        /// Applies a named filter to the child element of this many-to-many.
        /// </overloads>
        /// <summary>
        /// Applies a named filter to the child element of this many-to-many.
        /// </summary>
        /// <param name="condition">The condition to apply</param>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        public ManyToManyPart<TChild> ApplyChildFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
        {
            var part = new FilterPart(new TFilter().Name, condition);
            childFilters.Add(part);
            return this;
        }

        /// <summary>
        /// Applies a named filter to the child element of this many-to-many.
        /// </summary>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        public ManyToManyPart<TChild> ApplyChildFilter<TFilter>() where TFilter : FilterDefinition, new()
        {
            return ApplyChildFilter<TFilter>(null);
        }

        /// <summary>
        /// Sets the where clause for this relationship, on the many-to-many element.
        /// </summary>
        public ManyToManyPart<TChild> ChildWhere(string where)
        {
            relationshipAttributes.Set(x => x.Where, where);
            return this;
        }

        protected override CollectionMapping GetCollectionMapping()
        {
            var collection = base.GetCollectionMapping();

            // key columns
            if (parentKeyColumns.Count == 0)
                collection.Key.AddDefaultColumn(new ColumnMapping { Name = entity.Name + "_id" });

            foreach (var column in parentKeyColumns)
                collection.Key.AddColumn(column);

            if (collection.Relationship != null)
            {
                // child columns
                if (childKeyColumns.Count == 0)
                    ((ManyToManyMapping)collection.Relationship).AddDefaultColumn(new ColumnMapping {Name = typeof(TChild).Name + "_id"});

                foreach (var column in childKeyColumns)
                    ((ManyToManyMapping)collection.Relationship).AddColumn(column);
            }

            // HACK: Index only on list and map - shouldn't have to do this!
            if (index != null)
            {
#pragma warning disable 612,618
                collection.Index = index.GetIndexMapping();
#pragma warning restore 612,618
            }

            // HACK: shouldn't have to do this!
            if (manyToManyIndex != null && collection.Collection == Collection.Map)
#pragma warning disable 612,618
                collection.Index = manyToManyIndex.GetIndexMapping();
#pragma warning restore 612,618

            return collection;
        }
    }
}
