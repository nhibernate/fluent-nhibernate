using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Identity
{
    [Serializable]
    public class KeyPropertyMapping : MappingBase, ICompositeIdKeyMapping
    {
        readonly AttributeStore attributes = new AttributeStore();
        readonly IList<ColumnMapping> columns = new List<ColumnMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessKeyProperty(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public string Access
        {
            get { return attributes.GetOrDefault<string>("Access"); }
        }

        public TypeReference Type
        {
            get { return attributes.GetOrDefault<TypeReference>("Type"); }
        }

        public int Length
        {
            get { return attributes.GetOrDefault<int>("Length"); }
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public Type ContainingEntityType { get; set; }

        public void AddColumn(ColumnMapping mapping)
        {
            columns.Add(mapping);
        }

        public bool Equals(KeyPropertyMapping other)
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
            if (obj.GetType() != typeof(KeyPropertyMapping)) return false;
            return Equals((KeyPropertyMapping)obj);
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

        public void Set<T>(Expression<Func<KeyPropertyMapping, T>> expression, int layer, T value)
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