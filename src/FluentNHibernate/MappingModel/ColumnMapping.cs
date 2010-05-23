using System;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class ColumnMapping : MappingBase
    {
        private readonly AttributeStore<ColumnMapping> attributes;

        public ColumnMapping()
            : this(new AttributeStore())
        {}

        public ColumnMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ColumnMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessColumn(this);
        }

        public Member Member { get; set; }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public int Length
        {
            get { return attributes.Get(x => x.Length); }
            set { attributes.Set(x => x.Length, value); }
        }

        public bool NotNull
        {
            get { return attributes.Get(x => x.NotNull); }
            set { attributes.Set(x => x.NotNull, value); }
        }

        public bool Unique
        {
            get { return attributes.Get(x => x.Unique); }
            set { attributes.Set(x => x.Unique, value); }
        }

        public string UniqueKey
        {
            get { return attributes.Get(x => x.UniqueKey); }
            set { attributes.Set(x => x.UniqueKey, value); }
        }

        public string SqlType
        {
            get { return attributes.Get(x => x.SqlType); }
            set { attributes.Set(x => x.SqlType, value); }
        }

        public string Index
        {
            get { return attributes.Get(x => x.Index); }
            set { attributes.Set(x => x.Index, value); }
        }

        public string Check
        {
            get { return attributes.Get(x => x.Check); }
            set { attributes.Set(x => x.Check, value); }
        }

        public int Precision
        {
            get { return attributes.Get(x => x.Precision); }
            set { attributes.Set(x => x.Precision, value); }
        }

        public int Scale
        {
            get { return attributes.Get(x => x.Scale); }
            set { attributes.Set(x => x.Scale, value); }
        }

        public string Default
        {
            get { return attributes.Get(x => x.Default); }
            set { attributes.Set(x => x.Default, value); }
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ColumnMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ColumnMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        internal void MergeAttributes(AttributeStore<ColumnMapping> store)
        {
            attributes.Merge(store);
        }

        public ColumnMapping Clone()
        {
            return new ColumnMapping(attributes.CloneInner());
        }

        public bool Equals(ColumnMapping other)
        {
            return Equals(other.attributes, attributes) && Equals(other.Member, Member);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ColumnMapping)) return false;
            return Equals((ColumnMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((attributes != null ? attributes.GetHashCode() : 0) * 397) ^ (Member != null ? Member.GetHashCode() : 0);
            }
        }
    }
}