using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class ManyToManyMapping : MappingBase, ICollectionRelationshipMapping, IHasColumnMappings
    {
        private readonly AttributeStore<ManyToManyMapping> attributes;
        private readonly IDefaultableList<ColumnMapping> columns = new DefaultableList<ColumnMapping>();
        private readonly IList<FilterMapping> childFilters = new List<FilterMapping>();

        public IList<FilterMapping> ChildFilters
        {
            get { return childFilters; }
        }

        public ManyToManyMapping()
            : this(new AttributeStore())
        {}

        public ManyToManyMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ManyToManyMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessManyToMany(this);

            foreach (var column in columns)
                visitor.Visit(column);

            foreach (var filter in ChildFilters)
                visitor.Visit(filter);
        }

        public Type ChildType
        {
            get { return attributes.Get(x => x.ChildType); }
            set { attributes.Set(x => x.ChildType, value); }
        }

        public Type ParentType
        {
            get { return attributes.Get(x => x.ParentType); }
            set { attributes.Set(x => x.ParentType, value); }
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public string ForeignKey
        {
            get { return attributes.Get(x => x.ForeignKey); }
            set { attributes.Set(x => x.ForeignKey, value); }
        }

        public string Fetch
        {
            get { return attributes.Get(x => x.Fetch); }
            set { attributes.Set(x => x.Fetch, value); }
        }

        public string NotFound
        {
            get { return attributes.Get(x => x.NotFound); }
            set { attributes.Set(x => x.NotFound, value); }
        }

        public string Where
        {
            get { return attributes.Get(x => x.Where); }
            set { attributes.Set(x => x.Where, value); }
        }

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public string EntityName
        {
            get { return attributes.Get(x => x.EntityName); }
            set { attributes.Set(x => x.EntityName, value); }
        }

        public string OrderBy
        {
            get { return attributes.Get(x => x.OrderBy); }
            set { attributes.Set(x => x.OrderBy, value); }
        }        

        public string ChildPropertyRef
        {
            get { return attributes.Get(x => x.ChildPropertyRef); }
            set { attributes.Set(x => x.ChildPropertyRef, value); }
        }

        public IDefaultableEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public Type ContainingEntityType { get; set; }

        public void AddColumn(ColumnMapping column)
        {
            columns.Add(column);
        }

        public void AddDefaultColumn(ColumnMapping column)
        {
            columns.AddDefault(column);
        }

        public void ClearColumns()
        {
            columns.Clear();
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool IsSpecified<TResult>(Expression<Func<ManyToManyMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ManyToManyMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ManyToManyMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
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
    }
}
