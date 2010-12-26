using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class IndexManyToManyPart
    {
        private readonly Type entity;
        private readonly IList<string> columns = new List<string>();
        private readonly AttributeStore<IndexManyToManyMapping> attributes = new AttributeStore<IndexManyToManyMapping>();

        public IndexManyToManyPart(Type entity)
        {
            this.entity = entity;
        }

        public IndexManyToManyPart Column(string indexColumnName)
        {
            columns.Add(indexColumnName);
            return this;
        }

        public IndexManyToManyPart Type<TIndex>()
        {
            attributes.Set(x => x.Class, new TypeReference(typeof(TIndex)));
            return this;
        }

        public IndexManyToManyPart Type(Type indexType)
        {
            attributes.Set(x => x.Class, new TypeReference(indexType));
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public IndexManyToManyPart EntityName(string entityName)
        {
            attributes.Set(x => x.EntityName, entityName);
            return this;
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        public IndexManyToManyMapping GetIndexMapping()
        {
            var mapping = new IndexManyToManyMapping(attributes.CloneInner());

            mapping.ContainingEntityType = entity;

            columns.Each(x => mapping.AddColumn(new ColumnMapping { Name = x }));

            return mapping;
        }
    }
}