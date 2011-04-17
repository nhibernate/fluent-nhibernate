using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class IndexMapping : MappingBase, IIndexMapping, IHasColumnMappings
    {
        readonly AttributeStore attributes;
        readonly LayeredColumns columns = new LayeredColumns();

        public IndexMapping()
            : this(new AttributeStore())
        {}

        public IndexMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessIndex(this);

            foreach (var column in Columns)
                visitor.Visit(column);
        }

        public TypeReference Type
        {
            get { return attributes.GetOrDefault<TypeReference>("Type"); }
        }

        public int Offset
        {
            get { return attributes.GetOrDefault<int>("Offset"); }
        }

        public Type ContainingEntityType { get; set; }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns.Columns; }
        }

        public void AddColumn(int layer, ColumnMapping mapping)
        {
            columns.AddColumn(layer, mapping);
        }

        public void MakeColumnsEmpty(int layer)
        {
            columns.MakeColumnsEmpty(layer);
        }

        public bool Equals(IndexMapping other)
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
            if (obj.GetType() != typeof(IndexMapping)) return false;
            return Equals((IndexMapping)obj);
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

        public void Set<T>(Expression<Func<IndexMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }
    }
}
