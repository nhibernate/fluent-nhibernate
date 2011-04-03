using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Identity
{
    [Serializable]
    public class KeyPropertyMapping : MappingBase, ICompositeIdKeyMapping
    {
        private readonly AttributeStore<KeyPropertyMapping> attributes = new AttributeStore<KeyPropertyMapping>();
        private readonly IList<ColumnMapping> columns = new List<ColumnMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessKeyProperty(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public TypeReference Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public int Length
        {
            get { return attributes.Get(x => x.Length); }
            set { attributes.Set(x => x.Length, value); }
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

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<KeyPropertyMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<KeyPropertyMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
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
    }
}