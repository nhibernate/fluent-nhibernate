using System;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class KeyMapping : MappingBase, IHasColumnMappings
    {
        private readonly AttributeStore<KeyMapping> attributes;
        private readonly IDefaultableList<ColumnMapping> columns = new DefaultableList<ColumnMapping>();
        public Type ContainingEntityType { get; set; }

        public KeyMapping()
            : this(new AttributeStore())
        {}

        public KeyMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<KeyMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessKey(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public string ForeignKey
        {
            get { return attributes.Get(x => x.ForeignKey); }
            set { attributes.Set(x => x.ForeignKey, value); }
        }

        public string PropertyRef
        {
            get { return attributes.Get(x => x.PropertyRef); }
            set { attributes.Set(x => x.PropertyRef, value); }
        }

        public string OnDelete
        {
            get { return attributes.Get(x => x.OnDelete); }
            set { attributes.Set(x => x.OnDelete, value); }
        }

        public bool NotNull
        {
            get { return attributes.Get(x => x.NotNull); }
            set { attributes.Set(x => x.NotNull, value); }
        }

        public bool Update
        {
            get { return attributes.Get(x => x.Update); }
            set { attributes.Set(x => x.Update, value); }
        }

        public bool Unique
        {
            get { return attributes.Get(x => x.Unique); }
            set { attributes.Set(x => x.Unique, value); }
        }

        public IDefaultableEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public void AddColumn(ColumnMapping mapping)
        {
            if (columns.Contains(mapping))
                return;

            columns.Add(mapping);
        }

        public void AddDefaultColumn(ColumnMapping mapping)
        {
            if (columns.Contains(mapping))
                return;

            columns.AddDefault(mapping);
        }

        public void ClearColumns()
        {
            columns.ClearAll();
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<KeyMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<KeyMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public bool Equals(KeyMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes) &&
                other.columns.ContentEquals(columns) &&
                Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(KeyMapping)) return false;
            return Equals((KeyMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (columns != null ? columns.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }
    }
}