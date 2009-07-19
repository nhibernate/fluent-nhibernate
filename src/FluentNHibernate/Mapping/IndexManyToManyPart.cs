using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class IndexManyToManyPart
    {
        private readonly IndexManyToManyMapping mapping = new IndexManyToManyMapping();

        public IndexManyToManyPart Column(string indexColumnName)
        {
            mapping.AddColumn(new ColumnMapping { Name = indexColumnName });
            return this;
        }

        public IndexManyToManyPart Type<TIndex>()
        {
            mapping.Class = new TypeReference(typeof(TIndex));
            return this;
        }

        public IndexManyToManyMapping GetIndexMapping()
        {
            return mapping;
        }
    }
}