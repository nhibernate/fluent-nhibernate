using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping.Builders;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class ManyToManyPart<TChild> : ToManyBase<ManyToManyPart<TChild>, TChild, ManyToManyMapping>
    {
        private readonly IList<FilterMapping> childFilters = new List<FilterMapping>();
        private readonly FetchTypeExpression<ManyToManyPart<TChild>> fetch;
        private readonly NotFoundExpression<ManyToManyPart<TChild>> notFound;
        readonly AttributeStore sharedColumnAttributes = new AttributeStore();

        public ManyToManyPart(Type entity, Member property)
            : this(entity, property, property.PropertyType)
        {
        }

        protected ManyToManyPart(Type entity, Member member, Type collectionType)
            : base(entity, member, collectionType)
        {
            fetch = new FetchTypeExpression<ManyToManyPart<TChild>>(this, value => collectionAttributes.Set(x => x.Fetch, value));
            notFound = new NotFoundExpression<ManyToManyPart<TChild>>(this, value => relationshipMapping.NotFound = value);

            relationshipMapping = new ManyToManyMapping
            {
                ContainingEntityType = entity
            };
            relationshipMapping.As<ManyToManyMapping>(x =>
                x.AddDefaultColumn(new ColumnMapping { Name = typeof(TChild).Name + "_id"}));
        }

        /// <summary>
        /// Sets a single child key column. If there are multiple columns, use ChildKeyColumns.Add
        /// </summary>
        public ManyToManyPart<TChild> ChildKeyColumn(string childKeyColumn)
        {
            relationshipMapping.As<ManyToManyMapping>(x =>
            {
                x.ClearColumns();
                x.AddColumn(new ColumnMapping {Name = childKeyColumn});
            });
            return this;
        }

        /// <summary>
        /// Sets a single parent key column. If there are multiple columns, use ParentKeyColumns.Add
        /// </summary>
        public ManyToManyPart<TChild> ParentKeyColumn(string parentKeyColumn)
        {
            Key(ke => ke.Column(parentKeyColumn));
            return this;
        }

        public ColumnMappingCollection<ManyToManyPart<TChild>> ChildKeyColumns
        {
            get { return new ColumnMappingCollection<ManyToManyPart<TChild>>(this, relationshipMapping as ManyToManyMapping, sharedColumnAttributes); }
        }

        public ColumnMappingCollection<ManyToManyPart<TChild>> ParentKeyColumns
        {
            get { return new ColumnMappingCollection<ManyToManyPart<TChild>>(this, keyMapping, sharedColumnAttributes); }
        }

        public ManyToManyPart<TChild> ForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName)
        {
            Key(ke => ke.ForeignKey(parentForeignKeyName));
            relationshipMapping.As<ManyToManyMapping>(x => x.ForeignKey = childForeignKeyName);
            return this;
        }

        public ManyToManyPart<TChild> ChildPropertyRef(string childPropertyRef)
        {
            relationshipMapping.As<ManyToManyMapping>(x => x.ChildPropertyRef = childPropertyRef);
            return this;
        }

        public FetchTypeExpression<ManyToManyPart<TChild>> FetchType
        {
            get { return fetch; }
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
            relationshipMapping.As<ManyToManyMapping>(x =>
            {
                foreach (var filterMapping in childFilters)
                    x.ChildFilters.Add(filterMapping);
            });

            return relationshipMapping;
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
            relationshipMapping.As<ManyToManyMapping>(x => x.OrderBy = orderBy);
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
            var filterMapping = new FilterMapping();
            var builder = new FilterBuilder(filterMapping);
            builder.Name(name);
            builder.Condition(condition);
            childFilters.Add(filterMapping);
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
            return ApplyChildFilter(new TFilter().Name, condition);
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
            relationshipMapping.As<ManyToManyMapping>(x => x.Where = where);
            return this;
        }

        protected override ICollectionMapping GetCollectionMapping()
        {
            var collection = base.GetCollectionMapping();

            // HACK: Index only on list and map - shouldn't have to do this!
            if (indexMapping != null && collection is IIndexedCollectionMapping)
                ((IIndexedCollectionMapping)collection).Index = indexMapping;

            return collection;
        }
    }
}
