using System.Linq;

namespace FluentNHibernate.MappingModel
{
    public abstract class ColumnBasedMappingBase : MappingBase, IHasColumnMappings
    {
        private readonly string[] columnAttributes = new[] { "Length", "Precision", "Scale", "NotNull", "Unique", "UniqueKey", "SqlType", "Index", "Check", "Default" };
        protected readonly IDefaultableList<ColumnMapping> columns = new DefaultableList<ColumnMapping>();
        protected readonly AttributeStore attributes;

        protected ColumnBasedMappingBase(AttributeStore underlyingStore)
        {
            attributes = underlyingStore.Clone();
        }

        public override bool IsSpecified(string property)
        {
            if (columnAttributes.Contains(property))
                return columns.Any(x => x.IsSpecified(property));

            return attributes.IsSpecified(property);
        }

        public bool HasValue(string property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(string property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public IDefaultableEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public void AddColumn(ColumnMapping mapping)
        {
            columns.Add(mapping);
        }

        public void AddDefaultColumn(ColumnMapping mapping)
        {
            columns.AddDefault(mapping);
        }

        public void ClearColumns()
        {
            columns.Clear();
        }
    }
}