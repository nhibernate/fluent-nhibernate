using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class IndexPart
    {
        readonly Type entity;
        readonly List<string> columns = new List<string>();
        readonly AttributeStore attributes = new AttributeStore();

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
            attributes.Set("Type", Layer.UserSupplied, new TypeReference(typeof(TIndex)));
            return this;
        }

        public IndexPart Type(Type type)
        {
            attributes.Set("Type", Layer.UserSupplied, new TypeReference(type));
            return this;
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        public IndexMapping GetIndexMapping()
        {
            var mapping = new IndexMapping(attributes.Clone());

            mapping.ContainingEntityType = entity;

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
