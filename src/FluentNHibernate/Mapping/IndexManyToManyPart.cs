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
        private readonly AttributeStore<IndexMapping> attributes = new AttributeStore<IndexMapping>();

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
            attributes.Set(x => x.Type, new TypeReference(typeof(TIndex)));
            return this;
        }

        public IndexManyToManyPart Type(Type indexType)
        {
            attributes.Set(x => x.Type, new TypeReference(indexType));
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

        public IndexMapping GetIndexMapping()
        {
            var mapping = new IndexMapping(attributes.CloneInner());

            mapping.IsManyToMany = true;
            mapping.ContainingEntityType = entity;

            columns.Each(x => mapping.AddColumn(new ColumnMapping { Name = x }));

            return mapping;
        }
    }
}