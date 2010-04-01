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
        private readonly Type entity;
        private readonly FetchTypeExpression<ManyToManyPart<TChild>> fetch;
        private readonly NotFoundExpression<ManyToManyPart<TChild>> notFound;
        private IndexManyToManyPart manyToManyIndex;
        private IndexPart index;
        private readonly IList<string> childColumns = new List<string>();
        private readonly IList<string> parentColumns = new List<string>();
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
        }

        public override ICollectionMapping GetCollectionMapping()
        {
            var collection = base.GetCollectionMapping();

            // key columns
            if (parentColumns.Count == 0)
                collection.Key.AddDefaultColumn(new ColumnMapping { Name = entity.Name + "_id" });

            foreach (var column in parentColumns)
                collection.Key.AddColumn(new ColumnMapping { Name = column });

            if (collection.Relationship != null)
            {
                // child columns
                if (childColumns.Count == 0)
                    ((ManyToManyMapping)collection.Relationship).AddDefaultColumn(new ColumnMapping {Name = typeof(TChild).Name + "_id"});

                foreach (var column in childColumns)
                    ((ManyToManyMapping)collection.Relationship).AddColumn(new ColumnMapping {Name = column});
            }

            // HACK: Index only on list and map - shouldn't have to do this!
            if (index != null && collection is IIndexedCollectionMapping)
                ((IIndexedCollectionMapping)collection).Index = index.GetIndexMapping();

            // HACK: shouldn't have to do this!
            if (manyToManyIndex != null && collection is MapMapping)
                ((MapMapping)collection).Index = manyToManyIndex.GetIndexMapping();

            return collection;
        }

        public ManyToManyPart<TChild> ChildKeyColumn(string childKeyColumn)
        {
            childColumns.Clear(); // support only one currently
            childColumns.Add(childKeyColumn);
            return this;
        }

        public ManyToManyPart<TChild> ParentKeyColumn(string parentKeyColumn)
        {
            parentColumns.Clear(); // support only one currently
            parentColumns.Add(parentKeyColumn);
            return this;
        }

        public ManyToManyPart<TChild> ForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName)
        {
            keyMapping.ForeignKey = parentForeignKeyName;
            relationshipAttributes.Set(x => x.ForeignKey, childForeignKeyName);
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
            EnsureGenericDictionary();

            var indexType = typeof(TChild).GetGenericArguments()[0];
            var valueType = typeof(TChild).GetGenericArguments()[1];

            manyToManyIndex = new IndexManyToManyPart(typeof(ManyToManyPart<TChild>));
            manyToManyIndex.Column(indexColumn);
            manyToManyIndex.Type(indexType);

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
            EnsureDictionary();

            manyToManyIndex = new IndexManyToManyPart(typeof(ManyToManyPart<TChild>));
            manyToManyIndex.Column(indexColumn);
            manyToManyIndex.Type(indexType);

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

            return mapping;
        }

        /// <summary>
        /// Sets the order-by clause for this one-to-many relationship.
        /// </summary>
        public ManyToManyPart<TChild> OrderBy(string orderBy)
        {
            collectionAttributes.Set(x => x.OrderBy, orderBy);
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
    }
}
