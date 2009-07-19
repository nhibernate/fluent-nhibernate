using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class IndexPart
    {
        private readonly IndexMapping mapping = new IndexMapping();

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
            return mapping;
        }
    }
}