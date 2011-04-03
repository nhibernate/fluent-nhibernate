using System;
using System.Linq;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public abstract class ColumnBasedMappingBase : MappingBase, IHasColumnMappings
    {
        readonly string[] columnAttributes = new[] { "Length", "Precision", "Scale", "NotNull", "Unique", "UniqueKey", "SqlType", "Index", "Check", "Default" };
        readonly IDefaultableList<ColumnMapping> columns = new DefaultableList<ColumnMapping>();
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
            columns.ClearAll();
        }

        public bool Equals(ColumnBasedMappingBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.columns.ContentEquals(columns) &&
                Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ColumnBasedMappingBase)) return false;
            return Equals((ColumnBasedMappingBase)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((columns != null ? columns.GetHashCode() : 0) * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
            }
        }
    }
}