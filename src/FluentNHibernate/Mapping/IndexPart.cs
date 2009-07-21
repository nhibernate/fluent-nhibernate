using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class IndexPart
    {
        private readonly IndexMapping mapping = new IndexMapping();
        private readonly Type entity;
        
        public IndexPart(Type entity)
        {
            this.entity = entity;
        }

        public IndexPart Column(string indexColumnName)
        {
            mapping.AddColumn(new ColumnMapping { Name = indexColumnName });
            return this;
        }

        public IndexPart Type<TIndex>()
        {
            mapping.Type = new TypeReference(typeof(TIndex));
            return this;
        }

        public IndexMapping GetIndexMapping()
        {
            mapping.ContainingEntityType = entity;
            return mapping;
        }
    }
}