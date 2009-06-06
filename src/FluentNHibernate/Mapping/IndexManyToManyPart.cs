using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class IndexManyToManyPart
    {
        private readonly IndexManyToManyMapping mapping = new IndexManyToManyMapping();

        public IndexManyToManyPart WithColumn(string indexColumnName)
        {
            mapping.AddColumn(new ColumnMapping { Name = indexColumnName });
            return this;
        }

        public IndexManyToManyPart WithType<TIndex>()
        {
            mapping.Class = typeof(TIndex).AssemblyQualifiedName;
            return this;
        }

        public IndexManyToManyMapping GetIndexMapping()
        {
            return mapping;
        }
    }
}