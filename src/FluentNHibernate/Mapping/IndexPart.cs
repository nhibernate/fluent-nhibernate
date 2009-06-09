using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class IndexPart
    {
        private readonly IndexMapping mapping = new IndexMapping();

        public IndexPart WithColumn(string indexColumnName)
        {
            mapping.AddColumn(new ColumnMapping { Name = indexColumnName });
            return this;
        }

        public IndexPart WithType<TIndex>()
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