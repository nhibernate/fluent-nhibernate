using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class IndexManyToManyPart
    {
        readonly Type entity;
        readonly IList<string> columns = new List<string>();
        readonly AttributeStore attributes = new AttributeStore();

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
            attributes.Set("Class", Layer.UserSupplied, new TypeReference(typeof(TIndex)));
            return this;
        }

        public IndexManyToManyPart Type(Type indexType)
        {
            attributes.Set("Class", Layer.UserSupplied, new TypeReference(indexType));
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public IndexManyToManyPart EntityName(string entityName)
        {
            attributes.Set("EntityName", Layer.UserSupplied, entityName);
            return this;
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        public IndexManyToManyMapping GetIndexMapping()
        {
            var mapping = new IndexManyToManyMapping(attributes.Clone())
            {
                ContainingEntityType = entity
            };

            columns.Each(name =>
            {
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, name);
                mapping.AddColumn(Layer.UserSupplied, columnMapping);
            });

            return mapping;
        }
    }
}