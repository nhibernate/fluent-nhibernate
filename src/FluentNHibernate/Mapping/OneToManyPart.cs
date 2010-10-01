using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping.Builders;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class OneToManyPart<TChild> : ToManyBase<OneToManyPart<TChild>, TChild, OneToManyMapping>
    {
        private readonly Type entity;
        private readonly CollectionCascadeExpression<OneToManyPart<TChild>> cascade;
        private readonly NotFoundExpression<OneToManyPart<TChild>> notFound;
        private readonly Type childType;

        public OneToManyPart(Type entity, Member property)
            : this(entity, property, property.PropertyType)
        {
        }

        protected OneToManyPart(Type entity, Member member, Type collectionType)
            : base(entity, member, collectionType)
        {
            this.entity = entity;
            childType = collectionType;

            cascade = new CollectionCascadeExpression<OneToManyPart<TChild>>(this, value => collectionAttributes.Set(x => x.Cascade, value));
            notFound = new NotFoundExpression<OneToManyPart<TChild>>(this, value => relationshipMapping.NotFound = value);

            collectionAttributes.SetDefault(x => x.Name, member.Name);

            relationshipMapping = new OneToManyMapping
            {
                ContainingEntityType = entity
            };
        }

        /// <summary>
        /// Specifies the behaviour for if this collection is not found
        /// </summary>
        public NotFoundExpression<OneToManyPart<TChild>> NotFound
        {
            get { return notFound; }
        }

        /// <summary>
        /// Specify the cascade behaviour
        /// </summary>
        public new CollectionCascadeExpression<OneToManyPart<TChild>> Cascade
        {
            get { return cascade; }
        }

        /// <summary>
        /// Specify the key column name
        /// </summary>
        /// <param name="columnName">Column name</param>
        public OneToManyPart<TChild> KeyColumn(string columnName)
        {
            Key(ke =>
            {
                ke.Columns.Clear();
                ke.Columns.Add(columnName);
            });
            return this;
        }

        /// <summary>
        /// Modify the key columns collection
        /// </summary>
        [Obsolete("Deprecated in favour of Key(ke => ke.Columns...)")]
        public ColumnMappingCollection<OneToManyPart<TChild>> KeyColumns
        {
            get { return new ColumnMappingCollection<OneToManyPart<TChild>>(this, new KeyBuilder(keyMapping).Columns); }
        }

        /// <summary>
        /// Specify a foreign key constraint
        /// </summary>
        /// <param name="foreignKeyName">Constraint name</param>
        public OneToManyPart<TChild> ForeignKeyConstraintName(string foreignKeyName)
        {
            return Key(ke => ke.ForeignKey(foreignKeyName));
        }

        /// <summary>
        /// Sets the order-by clause for this one-to-many relationship.
        /// </summary>
        public OneToManyPart<TChild> OrderBy(string orderBy)
        {
            collectionAttributes.Set(x => x.OrderBy, orderBy);
            return this;
        }

        /// <summary>
        /// Specify that this collection is read-only
        /// </summary>
        public OneToManyPart<TChild> ReadOnly()
        {
            collectionAttributes.Set(x => x.Mutable, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify a sub-select query for fetching this collection
        /// </summary>
        /// <param name="subselect">Query</param>
        public OneToManyPart<TChild> Subselect(string subselect)
        {
            collectionAttributes.Set(x => x.Subselect, subselect);
            return this;
        }

        /// <summary>
        /// Specify that the key is updatable
        /// </summary>
        [Obsolete("Deprecated in favour of Key(ke => ke.Update())")]
        public OneToManyPart<TChild> KeyUpdate()
        {
            Key(ke =>
            {
                if (nextBool)
                    ke.Update();
                else
                    ke.Not.Update();
            });
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify that the key is nullable
        /// </summary>
        [Obsolete("Deprecated in favour of Key(ke => ke.Nullable())")]
        public OneToManyPart<TChild> KeyNullable()
        {
            Key(ke =>
            {
                if (nextBool)
                    ke.Nullable();
                else
                    ke.Not.Nullable();
            });
            nextBool = true;
            return this;
        }

        protected override ICollectionRelationshipMapping GetRelationship()
        {
            return relationshipMapping;
        }
    }
}
