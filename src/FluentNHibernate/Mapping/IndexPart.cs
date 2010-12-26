using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class IndexPart
    {
        private readonly Type entity;
        private readonly List<string> columns = new List<string>();
        private readonly AttributeStore<IndexMapping> attributes = new AttributeStore<IndexMapping>();

        public IndexPart(Type entity)
        {
            this.entity = entity;
        }

        public IndexPart Column(string indexColumnName)
        {
            columns.Add(indexColumnName);
            return this;
        }

        public IndexPart Type<TIndex>()
        {
            attributes.Set(x => x.Type, new TypeReference(typeof(TIndex)));
            return this;
        }

        public IndexPart Type(Type type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        public IndexMapping GetIndexMapping()
        {
            var mapping = new IndexMapping(attributes.CloneInner());

            mapping.ContainingEntityType = entity;

            columns.Each(x => mapping.AddColumn(new ColumnMapping { Name = x }));

            return mapping;
        }
    }
}
