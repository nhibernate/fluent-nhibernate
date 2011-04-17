using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class KeyMapping : MappingBase, IHasColumnMappings
    {
        readonly AttributeStore attributes;
        readonly LayeredColumns columns = new LayeredColumns();
        public Type ContainingEntityType { get; set; }

        public KeyMapping()
            : this(new AttributeStore())
        {}

        public KeyMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessKey(this);

            foreach (var column in Columns)
                visitor.Visit(column);
        }

        public string ForeignKey
        {
            get { return attributes.GetOrDefault<string>("ForeignKey"); }
        }

        public string PropertyRef
        {
            get { return attributes.GetOrDefault<string>("PropertyRef"); }
        }

        public string OnDelete
        {
            get { return attributes.GetOrDefault<string>("OnDelete"); }
        }

        public bool NotNull
        {
            get { return attributes.GetOrDefault<bool>("NotNull"); }
        }

        public bool Update
        {
            get { return attributes.GetOrDefault<bool>("Update"); }
        }

        public bool Unique
        {
            get { return attributes.GetOrDefault<bool>("Unique"); }
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

        public void Set<T>(Expression<Func<KeyMapping, T>> expression, int layer, T value)
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