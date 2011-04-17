using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class ElementMapping : MappingBase, IHasColumnMappings
    {
        readonly LayeredColumns columns = new LayeredColumns();
        readonly AttributeStore attributes;

        public ElementMapping()
            : this(new AttributeStore())
        { }

        public ElementMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessElement(this);

            foreach (var column in Columns)
                visitor.Visit(column);
        }

        public TypeReference Type
        {
            get { return attributes.GetOrDefault<TypeReference>("Type"); }
        }

        public string Formula
        {
            get { return attributes.GetOrDefault<string>("Formula"); }
        }

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

        public Type ContainingEntityType { get; set; }

        public bool Equals(ElementMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.columns.ContentEquals(columns) &&
                Equals(other.attributes, attributes) &&
                Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ElementMapping)) return false;
            return Equals((ElementMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (columns != null ? columns.GetHashCode() : 0);
                result = (result * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }

        public void Set<T>(Expression<Func<ElementMapping, T>> expression, int layer, T value)
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