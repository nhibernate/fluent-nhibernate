using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class ManyToManyMapping : MappingBase, ICollectionRelationshipMapping, IHasColumnMappings
    {
        readonly AttributeStore attributes;
        readonly LayeredColumns columns = new LayeredColumns();
        readonly IList<FilterMapping> childFilters = new List<FilterMapping>();

        public IList<FilterMapping> ChildFilters
        {
            get { return childFilters; }
        }

        public ManyToManyMapping()
            : this(new AttributeStore())
        {}

        public ManyToManyMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessManyToMany(this);

            foreach (var column in Columns)
                visitor.Visit(column);

            foreach (var filter in ChildFilters)
                visitor.Visit(filter);
        }

        public Type ChildType
        {
            get { return attributes.GetOrDefault<Type>("ChildType"); }
        }

        public Type ParentType
        {
            get { return attributes.GetOrDefault<Type>("ParentType"); }
        }

        public TypeReference Class
        {
            get { return attributes.GetOrDefault<TypeReference>("Class"); }
        }

        public string ForeignKey
        {
            get { return attributes.GetOrDefault<string>("ForeignKey"); }
        }

        public string Fetch
        {
            get { return attributes.GetOrDefault<string>("Fetch"); }
        }

        public string NotFound
        {
            get { return attributes.GetOrDefault<string>("NotFound"); }
        }

        public string Where
        {
            get { return attributes.GetOrDefault<string>("Where"); }
        }

        public bool Lazy
        {
            get { return attributes.GetOrDefault<bool>("Lazy"); }
        }

        public string EntityName
        {
            get { return attributes.GetOrDefault<string>("EntityName"); }
        }

        public string OrderBy
        {
            get { return attributes.GetOrDefault<string>("OrderBy"); }
        }        

        public string ChildPropertyRef
        {
            get { return attributes.GetOrDefault<string>("ChildPropertyRef"); }
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

        public bool Equals(ManyToManyMapping other)
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
            if (obj.GetType() != typeof(ManyToManyMapping)) return false;
            return Equals((ManyToManyMapping)obj);
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

        public void Set<T>(Expression<Func<ManyToManyMapping, T>> expression, int layer, T value)
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
